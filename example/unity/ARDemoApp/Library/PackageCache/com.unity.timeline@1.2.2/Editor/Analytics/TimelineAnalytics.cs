using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Analytics
{
    class TimelineSceneInfo
    {
        public Dictionary<string, int> trackCount = new Dictionary<string, int>
        {
            {"ActivationTrack", 0},
            {"AnimationTrack", 0},
            {"AudioTrack", 0},
            {"ControlTrack", 0},
            {"PlayableTrack", 0},
            {"UserType", 0},
            {"Other", 0}
        };

        public Dictionary<string, int> userTrackTypesCount = new Dictionary<string, int>();
        public HashSet<TimelineAsset> uniqueDirectors = new HashSet<TimelineAsset>();
        public int numTracks = 0;
        public int minDuration = int.MaxValue;
        public int maxDuration = int.MinValue;
        public int minNumTracks = int.MaxValue;
        public int maxNumTracks = int.MinValue;
        public int numRecorded  = 0;
    }

    [Serializable]
    struct TrackInfo
    {
        public string name;
        public double percent;
    }

    [Serializable]
    class TimelineEventInfo
    {
        public int num_timelines;
        public int min_duration, max_duration;
        public int min_num_tracks, max_num_tracks;
        public double recorded_percent;
        public List<TrackInfo> track_info = new List<TrackInfo>();
        public string most_popular_user_track = string.Empty;

        public TimelineEventInfo(TimelineSceneInfo sceneInfo)
        {
            num_timelines = sceneInfo.uniqueDirectors.Count;
            min_duration = sceneInfo.minDuration;
            max_duration = sceneInfo.maxDuration;
            min_num_tracks = sceneInfo.minNumTracks;
            max_num_tracks = sceneInfo.maxNumTracks;
            recorded_percent = Math.Round(100.0 * sceneInfo.numRecorded / sceneInfo.numTracks, 1);

            foreach (KeyValuePair<string, int> kv in sceneInfo.trackCount.Where(x => x.Value > 0))
            {
                track_info.Add(new TrackInfo()
                {
                    name = kv.Key,
                    percent = Math.Round(100.0 * kv.Value / sceneInfo.numTracks, 1)
                });
            }

            if (sceneInfo.userTrackTypesCount.Any())
            {
                most_popular_user_track = sceneInfo.userTrackTypesCount
                    .First(x => x.Value == sceneInfo.userTrackTypesCount.Values.Max()).Key;
            }
        }

        public static bool IsUserType(Type t)
        {
            string nameSpace = t.Namespace;
            return string.IsNullOrEmpty(nameSpace) || !nameSpace.StartsWith("UnityEngine.Timeline");
        }
    }


    static class TimelineAnalytics
    {
        static TimelineSceneInfo _timelineSceneInfo = new TimelineSceneInfo();

        class TimelineAnalyticsPreProcess : IPreprocessBuildWithReport
        {
            public int callbackOrder { get { return 0; }  }
            public void OnPreprocessBuild(BuildReport report)
            {
                _timelineSceneInfo = new TimelineSceneInfo();
            }
        }

        class TimelineAnalyticsProcess : IProcessSceneWithReport
        {
            public int callbackOrder
            {
                get { return 0; }
            }

            public void OnProcessScene(Scene scene, BuildReport report)
            {
                var timelines = UnityEngine.Object.FindObjectsOfType<PlayableDirector>().Select(pd => pd.playableAsset).OfType<TimelineAsset>().Distinct();

                foreach (var timeline in timelines)
                {
                    if (_timelineSceneInfo.uniqueDirectors.Add(timeline))
                    {
                        _timelineSceneInfo.numTracks += timeline.flattenedTracks.Count();
                        _timelineSceneInfo.minDuration = Math.Min(_timelineSceneInfo.minDuration, (int)(timeline.duration * 1000));
                        _timelineSceneInfo.maxDuration = Math.Max(_timelineSceneInfo.maxDuration, (int)(timeline.duration * 1000));
                        _timelineSceneInfo.minNumTracks = Math.Min(_timelineSceneInfo.minNumTracks, timeline.flattenedTracks.Count());
                        _timelineSceneInfo.maxNumTracks = Math.Max(_timelineSceneInfo.maxNumTracks, timeline.flattenedTracks.Count());

                        foreach (var track in timeline.flattenedTracks)
                        {
                            string key = track.GetType().Name;
                            if (_timelineSceneInfo.trackCount.ContainsKey(key))
                            {
                                _timelineSceneInfo.trackCount[key]++;
                            }
                            else
                            {
                                if (TimelineEventInfo.IsUserType(track.GetType()))
                                {
                                    _timelineSceneInfo.trackCount["UserType"]++;
                                    if (_timelineSceneInfo.userTrackTypesCount.ContainsKey(key))
                                        _timelineSceneInfo.userTrackTypesCount[key]++;
                                    else
                                        _timelineSceneInfo.userTrackTypesCount[key] = 1;
                                }
                                else
                                    _timelineSceneInfo.trackCount["Other"]++;
                            }

                            if (track.clips.Any(x => x.recordable))
                                _timelineSceneInfo.numRecorded++;
                            else
                            {
                                var animationTrack = track as AnimationTrack;
                                if (animationTrack != null)
                                {
                                    if (animationTrack.CanConvertToClipMode())
                                        _timelineSceneInfo.numRecorded++;
                                }
                            }
                        }
                    }
                }
            }
        }

        class TimelineAnalyticsPostProcess : IPostprocessBuildWithReport
        {
            public int callbackOrder {get { return 0; }}
            public void OnPostprocessBuild(BuildReport report)
            {
                if (_timelineSceneInfo.uniqueDirectors.Count > 0)
                {
                    var timelineEvent = new TimelineEventInfo(_timelineSceneInfo);
                    EditorAnalytics.SendEventTimelineInfo(timelineEvent);
                }
            }
        }
    }
}

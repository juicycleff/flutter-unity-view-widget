using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;
using ViewModelsMap = System.Collections.Generic.Dictionary<UnityEngine.Timeline.TimelineAsset, UnityEditor.Timeline.ScriptableObjectViewPrefs<UnityEditor.Timeline.TimelineAssetViewModel>>;
using ViewModelsList = System.Collections.Generic.List<UnityEditor.Timeline.ScriptableObjectViewPrefs<UnityEditor.Timeline.TimelineAssetViewModel>>;

namespace UnityEditor.Timeline
{
    static class TimelineWindowViewPrefs
    {
        public const string FilePath = "Library/Timeline";

        static readonly ViewModelsMap k_ViewModelsMap = new ViewModelsMap();
        static readonly ViewModelsList k_UnassociatedViewModels = new ViewModelsList();

        public static int viewModelCount
        {
            get { return k_ViewModelsMap.Count + k_UnassociatedViewModels.Count; }
        }

        public static TimelineAssetViewModel GetOrCreateViewModel(TimelineAsset asset)
        {
            if (asset == null)
                return CreateUnassociatedViewModel();

            ScriptableObjectViewPrefs<TimelineAssetViewModel> vm;
            if (k_ViewModelsMap.TryGetValue(asset, out vm))
                return vm.viewModel;

            return CreateViewModel(asset).viewModel;
        }

        public static TimelineAssetViewModel CreateUnassociatedViewModel()
        {
            var vm = new ScriptableObjectViewPrefs<TimelineAssetViewModel>(null, FilePath);
            k_UnassociatedViewModels.Add(vm);
            return vm.viewModel;
        }

        static ScriptableObjectViewPrefs<TimelineAssetViewModel> CreateViewModel(TimelineAsset asset)
        {
            var vm = new ScriptableObjectViewPrefs<TimelineAssetViewModel>(asset, FilePath);
            k_ViewModelsMap.Add(asset, vm);
            return vm;
        }

        public static void SaveViewModel(TimelineAsset asset)
        {
            if (asset == null)
                return;

            ScriptableObjectViewPrefs<TimelineAssetViewModel> vm;
            if (!k_ViewModelsMap.TryGetValue(asset, out vm))
                vm = CreateViewModel(asset);

            vm.Save();
        }

        public static void SaveAll()
        {
            foreach (var kvp in k_ViewModelsMap)
                kvp.Value.Save();
        }

        public static void UnloadViewModel(TimelineAsset asset)
        {
            ScriptableObjectViewPrefs<TimelineAssetViewModel> vm;
            if (k_ViewModelsMap.TryGetValue(asset, out vm))
            {
                vm.Dispose();
                k_ViewModelsMap.Remove(asset);
            }
        }

        public static void UnloadAllViewModels()
        {
            foreach (var kvp in k_ViewModelsMap)
                kvp.Value.Dispose();

            foreach (var vm in k_UnassociatedViewModels)
                vm.Dispose();

            k_ViewModelsMap.Clear();
            k_UnassociatedViewModels.Clear();
        }

        public static TrackViewModelData GetTrackViewModelData(TrackAsset track)
        {
            Debug.Assert(track != null);
            if (track == null)
                return new TrackViewModelData();

            Debug.Assert(track.timelineAsset != null);
            if (track.timelineAsset == null)
                return new TrackViewModelData();

            var prefs = GetOrCreateViewModel(track.timelineAsset);

            TrackViewModelData trackData;
            if (prefs.tracksViewModelData.TryGetValue(track, out trackData))
            {
                return trackData;
            }

            trackData = new TrackViewModelData();
            prefs.tracksViewModelData[track] = trackData;
            return trackData;
        }

        public static bool IsTrackCollapsed(TrackAsset track)
        {
            if (track == null)
                return true;

            return GetTrackViewModelData(track).collapsed;
        }

        public static void SetTrackCollapsed(TrackAsset track, bool collapsed)
        {
            if (track == null)
                return;

            GetTrackViewModelData(track).collapsed = collapsed;
        }

        public static bool IsShowMarkers(TrackAsset track)
        {
            if (track == null)
                return true;

            return GetTrackViewModelData(track).showMarkers;
        }

        public static void SetTrackShowMarkers(TrackAsset track, bool collapsed)
        {
            if (track == null)
                return;

            GetTrackViewModelData(track).showMarkers = collapsed;
        }

        public static bool GetShowInlineCurves(TrackAsset track)
        {
            if (track == null)
                return false;

            return GetTrackViewModelData(track).showInlineCurves;
        }

        public static void SetShowInlineCurves(TrackAsset track, bool inlineOn)
        {
            if (track == null)
                return;

            GetTrackViewModelData(track).showInlineCurves = inlineOn;
        }

        public static float GetInlineCurveHeight(TrackAsset asset)
        {
            if (asset == null)
                return TrackViewModelData.DefaultinlineAnimationCurveHeight;

            return GetTrackViewModelData(asset).inlineAnimationCurveHeight;
        }

        public static void SetInlineCurveHeight(TrackAsset asset, float height)
        {
            if (asset != null)
                GetTrackViewModelData(asset).inlineAnimationCurveHeight = height;
        }
    }
}

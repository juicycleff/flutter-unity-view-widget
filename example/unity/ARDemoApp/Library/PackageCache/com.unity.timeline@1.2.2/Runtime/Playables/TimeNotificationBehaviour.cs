using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Use this PlayableBehaviour to send notifications at a given time.
    /// </summary>
    /// <seealso cref="UnityEngine.Timeline.NotificationFlags"/>
    public class TimeNotificationBehaviour : PlayableBehaviour
    {
        struct NotificationEntry
        {
            public double time;
            public INotification payload;
            public bool notificationFired;
            public NotificationFlags flags;

            public bool triggerInEditor
            {
                get { return (flags & NotificationFlags.TriggerInEditMode) != 0; }
            }
            public bool prewarm
            {
                get { return (flags & NotificationFlags.Retroactive) != 0; }
            }
            public bool triggerOnce
            {
                get { return (flags & NotificationFlags.TriggerOnce) != 0; }
            }
        }

        readonly List<NotificationEntry> m_Notifications = new List<NotificationEntry>();
        double m_PreviousTime;
        bool m_NeedSortNotifications;

        Playable m_TimeSource;

        /// <summary>
        /// Sets an optional Playable that provides duration and Wrap mode information.
        /// </summary>
        /// <remarks>
        /// timeSource is optional. By default, the duration and Wrap mode will come from the current Playable.
        /// </remarks>
        public Playable timeSource
        {
            set { m_TimeSource = value; }
        }

        /// <summary>
        /// Creates and initializes a ScriptPlayable with a TimeNotificationBehaviour.
        /// </summary>
        /// <param name="graph">The playable graph.</param>
        /// <param name="duration">The duration of the playable.</param>
        /// <param name="loopMode">The loop mode of the playable.</param>
        /// <returns>A new TimeNotificationBehaviour linked to the PlayableGraph.</returns>
        public static ScriptPlayable<TimeNotificationBehaviour> Create(PlayableGraph graph, double duration, DirectorWrapMode loopMode)
        {
            var notificationsPlayable = ScriptPlayable<TimeNotificationBehaviour>.Create(graph);
            notificationsPlayable.SetDuration(duration);
            notificationsPlayable.SetTimeWrapMode(loopMode);
            notificationsPlayable.SetPropagateSetTime(true);
            return notificationsPlayable;
        }

        /// <summary>
        /// Adds a notification to be sent with flags, at a specific time.
        /// </summary>
        /// <param name="time">The time to send the notification.</param>
        /// <param name="payload">The notification.</param>
        /// <param name="flags">The notification flags that determine the notification behaviour. This parameter is set to Retroactive by default.</param>
        /// <seealso cref="UnityEngine.Timeline.NotificationFlags"/>
        public void AddNotification(double time, INotification payload, NotificationFlags flags = NotificationFlags.Retroactive)
        {
            m_Notifications.Add(new NotificationEntry
            {
                time = time,
                payload = payload,
                flags = flags
            });
            m_NeedSortNotifications = true;
        }

        /// <summary>
        /// This method is called when the PlayableGraph that owns this PlayableBehaviour starts.
        /// </summary>
        /// <param name="playable">The reference to the playable associated with this PlayableBehaviour.</param>
        public override void OnGraphStart(Playable playable)
        {
            SortNotifications();
            for (var i = 0; i < m_Notifications.Count; i++)
            {
                var notification = m_Notifications[i];
                notification.notificationFired = false;
                m_Notifications[i] = notification;
            }

            m_PreviousTime = playable.GetTime();
        }

        /// <summary>
        /// This method is called when the Playable play state is changed to PlayState.Paused
        /// </summary>
        /// <param name="playable">The reference to the playable associated with this PlayableBehaviour.</param>
        /// <param name="info">Playable context information such as weight, evaluationType, and so on.</param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (playable.IsDone())
            {
                SortNotifications();
                for (var i = 0; i < m_Notifications.Count; i++)
                {
                    var e = m_Notifications[i];
                    if (!e.notificationFired)
                    {
                        var duration = playable.GetDuration();
                        var canTrigger = m_PreviousTime <= e.time && e.time <= duration;
                        if (canTrigger)
                        {
                            Trigger_internal(playable, info.output, ref e);
                            m_Notifications[i] = e;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is called during the PrepareFrame phase of the PlayableGraph.
        /// </summary>
        /// <remarks>
        /// Called once before processing starts.
        /// </remarks>
        /// <param name="playable">The reference to the playable associated with this PlayableBehaviour.</param>
        /// <param name="info">Playable context information such as weight, evaluationType, and so on.</param>
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            // Never trigger on scrub
            if (info.evaluationType == FrameData.EvaluationType.Evaluate)
            {
                return;
            }

            SyncDurationWithExternalSource(playable);
            SortNotifications();
            var currentTime = playable.GetTime();

            // Fire notifications from previousTime till the end
            if (info.timeLooped)
            {
                var duration = playable.GetDuration();
                TriggerNotificationsInRange(m_PreviousTime, duration, info, playable, true);
                var dx = playable.GetDuration() - m_PreviousTime;
                var nFullTimelines = (int)((info.deltaTime * info.effectiveSpeed - dx) / playable.GetDuration());
                for (var i = 0; i < nFullTimelines; i++)
                {
                    TriggerNotificationsInRange(0, duration, info, playable, false);
                }
                TriggerNotificationsInRange(0, currentTime, info, playable, false);
            }
            else
            {
                var pt = playable.GetTime();
                TriggerNotificationsInRange(m_PreviousTime, pt, info,
                    playable, true);
            }

            for (var i = 0; i < m_Notifications.Count; ++i)
            {
                var e = m_Notifications[i];
                if (e.notificationFired && CanRestoreNotification(e, info, currentTime, m_PreviousTime))
                {
                    Restore_internal(ref e);
                    m_Notifications[i] = e;
                }
            }

            m_PreviousTime = playable.GetTime();
        }

        void SortNotifications()
        {
            if (m_NeedSortNotifications)
            {
                m_Notifications.Sort((x, y) => x.time.CompareTo(y.time));
                m_NeedSortNotifications = false;
            }
        }

        static bool CanRestoreNotification(NotificationEntry e, FrameData info, double currentTime, double previousTime)
        {
            if (e.triggerOnce)
                return false;
            if (info.timeLooped)
                return true;

            //case 1111595: restore the notification if the time is manually set before it
            return previousTime > currentTime && currentTime <= e.time;
        }

        void TriggerNotificationsInRange(double start, double end, FrameData info, Playable playable, bool checkState)
        {
            if (start <= end)
            {
                var playMode = Application.isPlaying;
                for (var i = 0; i < m_Notifications.Count; i++)
                {
                    var e = m_Notifications[i];
                    if (e.notificationFired && (checkState || e.triggerOnce))
                        continue;

                    var notificationTime = e.time;
                    if (e.prewarm && notificationTime < end && (e.triggerInEditor || playMode))
                    {
                        Trigger_internal(playable, info.output, ref e);
                        m_Notifications[i] = e;
                    }
                    else
                    {
                        if (notificationTime < start || notificationTime > end)
                            continue;

                        if (e.triggerInEditor || playMode)
                        {
                            Trigger_internal(playable, info.output, ref e);
                            m_Notifications[i] = e;
                        }
                    }
                }
            }
        }

        void SyncDurationWithExternalSource(Playable playable)
        {
            if (m_TimeSource.IsValid())
            {
                playable.SetDuration(m_TimeSource.GetDuration());
                playable.SetTimeWrapMode(m_TimeSource.GetTimeWrapMode());
            }
        }

        static void Trigger_internal(Playable playable, PlayableOutput output,  ref NotificationEntry e)
        {
            output.PushNotification(playable, e.payload);
            e.notificationFired = true;
        }

        static void Restore_internal(ref NotificationEntry e)
        {
            e.notificationFired = false;
        }
    }
}

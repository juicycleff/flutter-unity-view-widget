using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    static class NotificationUtilities
    {
        public static ScriptPlayable<TimeNotificationBehaviour> CreateNotificationsPlayable(PlayableGraph graph, IEnumerable<IMarker> markers, GameObject go)
        {
            var notificationPlayable = ScriptPlayable<TimeNotificationBehaviour>.Null;
            var director = go.GetComponent<PlayableDirector>();
            foreach (var e in markers)
            {
                var notif = e as INotification;
                if (notif == null)
                    continue;

                if (notificationPlayable.Equals(ScriptPlayable<TimeNotificationBehaviour>.Null))
                {
                    notificationPlayable = TimeNotificationBehaviour.Create(graph,
                        director.playableAsset.duration, director.extrapolationMode);
                }

                var time = (DiscreteTime)e.time;
                var tlDuration = (DiscreteTime)director.playableAsset.duration;
                if (time >= tlDuration && time <= tlDuration.OneTickAfter() && tlDuration != 0)
                {
                    time = tlDuration.OneTickBefore();
                }

                var notificationOptionProvider = e as INotificationOptionProvider;
                if (notificationOptionProvider != null)
                {
                    notificationPlayable.GetBehaviour().AddNotification((double)time, notif, notificationOptionProvider.flags);
                }
                else
                {
                    notificationPlayable.GetBehaviour().AddNotification((double)time, notif);
                }
            }

            return notificationPlayable;
        }

        public static bool TrackTypeSupportsNotifications(Type type)
        {
            var binding = (TrackBindingTypeAttribute)Attribute.GetCustomAttribute(type, typeof(TrackBindingTypeAttribute));
            return binding != null &&
                (typeof(Component).IsAssignableFrom(binding.type) ||
                    typeof(GameObject).IsAssignableFrom(binding.type));
        }
    }
}

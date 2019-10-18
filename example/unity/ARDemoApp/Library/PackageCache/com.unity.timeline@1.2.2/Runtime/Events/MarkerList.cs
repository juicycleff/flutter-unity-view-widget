using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [Serializable]
    struct MarkerList : ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] List<ScriptableObject> m_Objects;

        [HideInInspector, NonSerialized] List<IMarker> m_Cache;
        bool m_CacheDirty;
        bool m_HasNotifications;
        public List<IMarker> markers
        {
            get
            {
                BuildCache();
                return m_Cache;
            }
        }

        public MarkerList(int capacity)
        {
            m_Objects = new List<ScriptableObject>(capacity);
            m_Cache = new List<IMarker>(capacity);
            m_CacheDirty = true;
            m_HasNotifications = false;
        }

        public void Add(ScriptableObject item)
        {
            if (item == null)
                return;

            m_Objects.Add(item);
            m_CacheDirty = true;
        }

        public bool Remove(IMarker item)
        {
            if (!(item is ScriptableObject))
                throw new InvalidOperationException("Supplied type must be a ScriptableObject");
            return Remove((ScriptableObject)item, item.parent.timelineAsset, item.parent);
        }

        public bool Remove(ScriptableObject item, TimelineAsset timelineAsset, PlayableAsset thingToDirty)
        {
            if (!m_Objects.Contains(item)) return false;

            TimelineUndo.PushUndo(thingToDirty, "Delete Marker");
            m_Objects.Remove(item);
            m_CacheDirty = true;
            TimelineUndo.PushDestroyUndo(timelineAsset, thingToDirty, item, "Delete Marker");
            return true;
        }

        public void Clear()
        {
            m_Objects.Clear();
            m_CacheDirty = true;
        }

        public bool Contains(ScriptableObject item)
        {
            return m_Objects.Contains(item);
        }

        public IEnumerable<IMarker> GetMarkers()
        {
            return markers;
        }

        public int Count
        {
            get { return markers.Count; }
        }

        public IMarker this[int idx]
        {
            get
            {
                return markers[idx];
            }
        }

        public List<ScriptableObject> GetRawMarkerList()
        {
            return m_Objects;
        }

        public IMarker CreateMarker(Type type, double time, TrackAsset owner)
        {
            if (!typeof(ScriptableObject).IsAssignableFrom(type) || !typeof(IMarker).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    "The requested type needs to inherit from ScriptableObject and implement IMarker");
            }
            if (!owner.supportsNotifications && typeof(INotification).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    "Markers implementing the INotification interface cannot be added on tracks that do not support notifications");
            }

            var markerSO = ScriptableObject.CreateInstance(type);
            var marker = (IMarker)markerSO;
            marker.time = time;

            TimelineCreateUtilities.SaveAssetIntoObject(markerSO, owner);
            TimelineUndo.RegisterCreatedObjectUndo(markerSO, "Create " + type.Name);
            TimelineUndo.PushUndo(owner, "Create " + type.Name);

            Add(markerSO);
            marker.Initialize(owner);

            return marker;
        }

        public bool HasNotifications()
        {
            BuildCache();
            return m_HasNotifications;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
#if UNITY_EDITOR
            for (int i = m_Objects.Count - 1; i >= 0; i--)
            {
                object o = m_Objects[i];
                if (o == null)
                {
                    Debug.LogWarning("Empty marker found while loading timeline. It will be removed.");
                    m_Objects.RemoveAt(i);
                }
            }
#endif
            m_CacheDirty = true;
        }

        void BuildCache()
        {
            if (m_CacheDirty)
            {
                m_Cache = new List<IMarker>(m_Objects.Count);
                m_HasNotifications = false;
                foreach (var o in m_Objects)
                {
                    if (o != null)
                    {
                        m_Cache.Add(o as IMarker);
                        if (o is INotification)
                        {
                            m_HasNotifications = true;
                        }
                    }
                }

                m_CacheDirty = false;
            }
        }
    }
}

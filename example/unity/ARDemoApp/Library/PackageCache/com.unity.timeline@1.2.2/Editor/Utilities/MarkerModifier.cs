using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    static class MarkerModifier
    {
        public static void DeleteMarker(IMarker marker)
        {
            var trackAsset = marker.parent;
            if (trackAsset != null)
            {
                SelectionManager.Remove(marker);
                trackAsset.DeleteMarker(marker);
            }
        }

        public static IEnumerable<IMarker> CloneMarkersToParent(IEnumerable<IMarker> markers, TrackAsset parent)
        {
            if (!markers.Any()) return Enumerable.Empty<IMarker>();
            var clonedMarkers = new List<IMarker>();
            foreach (var marker in markers)
                clonedMarkers.Add(CloneMarkerToParent(marker, parent));
            return clonedMarkers;
        }

        public static IMarker CloneMarkerToParent(IMarker marker, TrackAsset parent)
        {
            var markerObject = marker as ScriptableObject;
            if (markerObject == null) return null;

            var newMarkerObject = Object.Instantiate(markerObject);
            AddMarkerToParent(newMarkerObject, parent);

            newMarkerObject.name = markerObject.name;
            try
            {
                CustomTimelineEditorCache.GetMarkerEditor((IMarker)newMarkerObject).OnCreate((IMarker)newMarkerObject, marker);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }


            return (IMarker)newMarkerObject;
        }

        static void AddMarkerToParent(ScriptableObject marker, TrackAsset parent)
        {
            TimelineCreateUtilities.SaveAssetIntoObject(marker, parent);
            TimelineUndo.RegisterCreatedObjectUndo(marker, "Duplicate Marker");
            TimelineUndo.PushUndo(parent, "Duplicate Marker");

            if (parent != null)
            {
                parent.AddMarker(marker);
                ((IMarker)marker).Initialize(parent);
            }
        }
    }
}

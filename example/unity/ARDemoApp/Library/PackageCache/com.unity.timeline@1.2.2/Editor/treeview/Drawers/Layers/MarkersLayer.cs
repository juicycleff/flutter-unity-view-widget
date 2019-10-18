using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class MarkersLayer : ItemsLayer
    {
        public MarkersLayer(Layer layerOrder, IRowGUI parent) : base(layerOrder)
        {
            CreateLists(parent);
        }

        void CreateLists(IRowGUI parent)
        {
            var markerCount = parent.asset.GetMarkerCount();
            if (markerCount == 0) return;

            var accumulator = new List<IMarker>();
            var sortedMarkers = new List<IMarker>(parent.asset.GetMarkers());
            var vm = TimelineWindowViewPrefs.GetTrackViewModelData(parent.asset);

            sortedMarkers.Sort((lhs, rhs) =>
            {
                // Sort by time first
                var timeComparison = lhs.time.CompareTo(rhs.time);
                if (timeComparison != 0)
                    return timeComparison;

                // If there's a collision, sort by edit timestamp
                var lhsObject = lhs as object;
                var rhsObject = rhs as object;

                if (lhsObject.Equals(null) || rhsObject.Equals(null))
                    return 0;

                var lhsHash = lhsObject.GetHashCode();
                var rhsHash = rhsObject.GetHashCode();

                if (vm.markerTimeStamps.ContainsKey(lhsHash) && vm.markerTimeStamps.ContainsKey(rhsHash))
                    return vm.markerTimeStamps[lhsHash].CompareTo(vm.markerTimeStamps[rhsHash]);

                return 0;
            });

            foreach (var current in sortedMarkers)
            {
                // TODO: Take zoom factor into account?
                if (accumulator.Count > 0 && Math.Abs(current.time - accumulator[accumulator.Count - 1].time) > TimeUtility.kTimeEpsilon)
                    ProcessAccumulator(accumulator, parent);

                accumulator.Add(current);
            }

            ProcessAccumulator(accumulator, parent);
        }

        void ProcessAccumulator(List<IMarker> accumulator, IRowGUI parent)
        {
            if (accumulator.Count == 0) return;

            if (accumulator.Count == 1)
            {
                AddItem(new TimelineMarkerGUI(accumulator[0], parent, this));
            }
            else
            {
                // Ensure that the cluster is always considered *below* the markers it contains.
                var clusterZOrder = Next();
                AddItem(
                    new TimelineMarkerClusterGUI(
                        accumulator.Select(m => new TimelineMarkerGUI(m, parent, this)).ToList(),
                        parent, this, clusterZOrder));
            }

            accumulator.Clear();
        }
    }
}

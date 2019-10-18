using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace UnityEditor.Timeline
{
    // Handles Undo animated properties on PlayableAssets from clips to create parameter animation

    static partial class TimelineRecording
    {
        internal static bool HasAnyPlayableAssetModifications(UndoPropertyModification[] modifications)
        {
            return modifications.Any(x => GetTarget(x) as IPlayableAsset != null);
        }

        internal static UndoPropertyModification[] ProcessPlayableAssetModification(UndoPropertyModification[] modifications, WindowState state)
        {
            // can't record without a director since the asset being modified might be a scene instance
            if (state == null || state.editSequence.director == null)
                return modifications;

            var remaining = new List<UndoPropertyModification>();
            foreach (UndoPropertyModification mod in modifications)
            {
                if (!ProcessPlayableAssetModification(mod, state))
                    remaining.Add(mod);
            }

            if (remaining.Count != modifications.Length)
            {
                state.rebuildGraph = true;
                state.GetWindow().Repaint();
            }

            return remaining.ToArray();
        }

        static bool ProcessPlayableAssetModification(UndoPropertyModification mod, WindowState state)
        {
            var target = GetTarget(mod) as IPlayableAsset;
            if (target == null)
                return false;

            var curvesOwner = AnimatedParameterUtility.ToCurvesOwner(target, state.editSequence.asset);
            if (curvesOwner == null || !curvesOwner.HasAnyAnimatableParameters())
                return false;

            return ProcessPlayableAssetRecording(mod, state, curvesOwner);
        }

        internal static TimelineClip FindClipWithAsset(TimelineAsset asset, IPlayableAsset target)
        {
            if (target == null || asset == null)
                return null;

            var clips = asset.flattenedTracks.SelectMany(x => x.clips);
            return clips.FirstOrDefault(x => x != null && x.asset != null && target == x.asset as IPlayableAsset);
        }

        static bool ProcessPlayableAssetRecording(UndoPropertyModification mod, WindowState state, ICurvesOwner curvesOwner)
        {
            if (mod.currentValue == null)
                return false;

            if (!curvesOwner.IsParameterAnimatable(mod.currentValue.propertyPath))
                return false;

            var localTime = state.editSequence.time;
            var timelineClip = curvesOwner as TimelineClip;
            if (timelineClip != null)
            {
                // don't use time global to local since it will possibly loop.
                localTime = timelineClip.ToLocalTimeUnbound(state.editSequence.time);
            }

            if (localTime < 0)
                return false;

            // grab the value from the current modification
            float fValue = 0;
            if (!float.TryParse(mod.currentValue.value, out fValue))
            {
                // case 916913 -- 'Add Key' menu item will passes 'True' or 'False' (instead of 1, 0)
                // so we need a special case to parse the boolean string
                bool bValue;
                if (!bool.TryParse(mod.currentValue.value, out bValue))
                {
                    Debug.Assert(false, "Invalid type in PlayableAsset recording");
                    return false;
                }

                fValue = bValue ? 1 : 0;
            }

            var added = curvesOwner.AddAnimatedParameterValueAt(mod.currentValue.propertyPath, fValue, (float)localTime);
            if (added && AnimationMode.InAnimationMode())
            {
                EditorCurveBinding binding = curvesOwner.GetCurveBinding(mod.previousValue.propertyPath);
                AnimationMode.AddPropertyModification(binding, mod.previousValue, true);
                curvesOwner.targetTrack.SetShowInlineCurves(true);
                if (state.GetWindow() != null && state.GetWindow().treeView != null)
                    state.GetWindow().treeView.CalculateRowRects();
            }

            return added;
        }

        static bool IsPlayableAssetProperty(SerializedProperty property)
        {
            return property.serializedObject.targetObject is IPlayableAsset;
        }
    }
}

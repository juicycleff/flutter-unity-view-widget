using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    class TimelineRecordingContextualResponder : IAnimationContextualResponder
    {
        public WindowState state { get; internal set; }

        public TimelineRecordingContextualResponder(WindowState _state)
        {
            state = _state;
        }

        //Unsupported stuff
        public bool HasAnyCandidates() { return false; }
        public bool HasAnyCurves() {return false; }
        public void AddCandidateKeys() {}
        public void AddAnimatedKeys() {}

        public bool IsAnimatable(PropertyModification[] modifications)
        {
            // search playable assets
            for (int i = 0; i < modifications.Length; i++)
            {
                var iAsset = modifications[i].target as IPlayableAsset;
                if (iAsset != null)
                {
                    var curvesOwner = AnimatedParameterUtility.ToCurvesOwner(iAsset, state.editSequence.asset);
                    if (curvesOwner != null && curvesOwner.HasAnyAnimatableParameters() && curvesOwner.IsParameterAnimatable(modifications[i].propertyPath))
                        return true;
                }
            }

            // search recordable game objects
            foreach (var gameObject in TimelineRecording.GetRecordableGameObjects(state))
            {
                for (int i = 0; i < modifications.Length; ++i)
                {
                    var modification = modifications[i];
                    if (AnimationWindowUtility.PropertyIsAnimatable(modification.target, modification.propertyPath, gameObject))
                        return true;
                }
            }

            return false;
        }

        public bool IsEditable(Object targetObject)
        {
            return true; // i.e. all animatable properties are editable
        }

        public bool KeyExists(PropertyModification[] modifications)
        {
            if (modifications.Length == 0 || modifications[0].target == null)
                return false;

            return TimelineRecording.HasKey(modifications, modifications[0].target, state);
        }

        public bool CandidateExists(PropertyModification[] modifications)
        {
            return true;
        }

        public bool CurveExists(PropertyModification[] modifications)
        {
            if (modifications.Length == 0 || modifications[0].target == null)
                return false;

            return TimelineRecording.HasCurve(modifications, modifications[0].target, state);
        }

        public void AddKey(PropertyModification[] modifications)
        {
            TimelineRecording.AddKey(modifications, state);
            state.Refresh();
        }

        public void RemoveKey(PropertyModification[] modifications)
        {
            if (modifications.Length == 0)
                return;

            var target = modifications[0].target;
            if (target == null)
                return;

            TimelineRecording.RemoveKey(modifications[0].target, modifications, state);

            var curvesOwner = target as ICurvesOwner;
            if (curvesOwner != null)
                curvesOwner.SanitizeCurvesData();

            state.Refresh();
        }

        public void RemoveCurve(PropertyModification[] modifications)
        {
            if (modifications.Length == 0)
                return;

            var target = modifications[0].target;
            if (target == null)
                return;

            TimelineRecording.RemoveCurve(target, modifications, state);

            var curvesOwner = target as ICurvesOwner;
            if (curvesOwner != null)
                curvesOwner.SanitizeCurvesData();

            state.Refresh();
        }

        public void GoToNextKeyframe(PropertyModification[] modifications)
        {
            if (modifications.Length == 0 || modifications[0].target == null)
                return;

            TimelineRecording.NextKey(modifications[0].target, modifications, state);
            state.Refresh();
        }

        public void GoToPreviousKeyframe(PropertyModification[] modifications)
        {
            TimelineRecording.PrevKey(modifications[0].target, modifications, state);
            state.Refresh();
        }
    }
}

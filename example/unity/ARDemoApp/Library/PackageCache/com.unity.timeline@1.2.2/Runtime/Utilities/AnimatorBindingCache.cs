#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Animator to Editor Curve Binding cache. Used to prevent frequent calls to GetAnimatorBindings which can be costly
    /// </summary>
    class AnimatorBindingCache
    {
        public const string TRPlaceHolder = "TransformTR";
        public const string ScalePlaceholder = "TransformScale";
        
        struct AnimatorEntry
        {
            public int animatorID;
            public bool applyRootMotion;
            public bool humanoid;
        }

        class AnimatorEntryComparer : IEqualityComparer<AnimatorEntry>
        {
            public bool Equals(AnimatorEntry x, AnimatorEntry y) { return x.animatorID == y.animatorID && x.applyRootMotion == y.applyRootMotion && x.humanoid == y.humanoid; }
            public int GetHashCode(AnimatorEntry obj) { return HashUtility.CombineHash(obj.animatorID, obj.applyRootMotion.GetHashCode(), obj.humanoid.GetHashCode()); }
            public static readonly AnimatorEntryComparer Instance = new AnimatorEntryComparer();
        }

        readonly Dictionary<AnimatorEntry, EditorCurveBinding[]> m_AnimatorCache = new Dictionary<AnimatorEntry, EditorCurveBinding[]>(AnimatorEntryComparer.Instance);
        readonly Dictionary<AnimationClip, EditorCurveBinding[]> m_ClipCache = new Dictionary<AnimationClip, EditorCurveBinding[]>();

        private static readonly EditorCurveBinding[] kEmptyArray = new EditorCurveBinding[0];
        private static readonly List<EditorCurveBinding> s_BindingScratchPad = new List<EditorCurveBinding>(1000);
         
        public AnimatorBindingCache()
        {
            AnimationUtility.onCurveWasModified += OnCurveWasModified;
        }
        
        public EditorCurveBinding[] GetAnimatorBindings(GameObject gameObject)
        {
            if (gameObject == null)
                return kEmptyArray;

            Animator animator = gameObject.GetComponent<Animator>();
            if (animator == null)
                return kEmptyArray;

            AnimatorEntry entry = new AnimatorEntry()
            {
                animatorID = animator.GetInstanceID(),
                applyRootMotion = animator.applyRootMotion,
                humanoid = animator.isHuman
            };

            EditorCurveBinding[] result = null;
            if (m_AnimatorCache.TryGetValue(entry, out result))
                return result;
          
            s_BindingScratchPad.Clear();

            // Replacement for AnimationMode.GetAnimatorBinding - this is faster and allocates kB instead of MB
            var transforms = animator.GetComponentsInChildren<Transform>();
            foreach (var t in transforms)
            {
                if (animator.IsBoneTransform(t))
                    s_BindingScratchPad.Add(EditorCurveBinding.FloatCurve(AnimationUtility.CalculateTransformPath(t,animator.transform), typeof(Transform), TRPlaceHolder));
            }

            var streamBindings = AnimationUtility.GetAnimationStreamBindings(animator.gameObject);
            UpdateTransformBindings(streamBindings);
            s_BindingScratchPad.AddRange(streamBindings);

            result = new EditorCurveBinding[s_BindingScratchPad.Count];
            s_BindingScratchPad.CopyTo(result);
            m_AnimatorCache[entry] = result;
            return result;
        }

        public EditorCurveBinding[] GetCurveBindings(AnimationClip clip)
        {
            if (clip == null)
                return kEmptyArray;

            EditorCurveBinding[] result;
            if (!m_ClipCache.TryGetValue(clip, out result))
            {
                result = AnimationMode.GetCurveBindings(clip);
                UpdateTransformBindings(result);
                m_ClipCache[clip] = result;
            }

            return result;

        }

        private static void UpdateTransformBindings(EditorCurveBinding[] bindings)
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                var binding = bindings[i];
                if (AnimationPreviewUtilities.IsRootMotion(binding))
                {
                    binding.type = typeof(Transform);
                    binding.propertyName = TRPlaceHolder;
                }
                else if (typeof(Transform).IsAssignableFrom(binding.type) && (binding.propertyName.StartsWith("m_LocalRotation.") ||binding.propertyName.StartsWith("m_LocalPosition.")))
                {
                    binding.propertyName = TRPlaceHolder; 
                }
                else if (typeof(Transform).IsAssignableFrom(binding.type) && binding.propertyName.StartsWith("m_LocalScale."))
                {
                    binding.propertyName = ScalePlaceholder;
                }
                bindings[i] = binding;
            }
        }

        public void Clear()
        {
            m_AnimatorCache.Clear();
            m_ClipCache.Clear();
        }

        void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType modification)
        {
            m_ClipCache.Remove(clip);
        }

    }
}
#endif

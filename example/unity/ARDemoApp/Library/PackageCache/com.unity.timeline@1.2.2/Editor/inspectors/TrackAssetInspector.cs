//#define PERF_PROFILE

using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(TrackAsset), true, isFallback = true)]
    [CanEditMultipleObjects]
    class TrackAssetInspector : Editor
    {
        class TrackCurvesWrapper : ICurvesOwnerInspectorWrapper
        {
            public ICurvesOwner curvesOwner { get; }
            public SerializedObject serializedPlayableAsset { get; }

            public int lastCurveVersion { get; set; }
            public double lastEvalTime { get; set; }

            public TrackCurvesWrapper(TrackAsset track)
            {
                lastCurveVersion = -1;
                lastEvalTime = -1;

                if (track != null)
                {
                    curvesOwner = track;
                    serializedPlayableAsset = new SerializedObject(track);
                }
            }

            public double ToLocalTime(double time)
            {
                return time;
            }
        }

        TrackCurvesWrapper m_TrackCurvesWrapper;

        SerializedProperty m_Name;
        bool m_IsBuiltInType;

        Texture m_HeaderIcon;


        protected TimelineWindow timelineWindow
        {
            get
            {
                return TimelineWindow.instance;
            }
        }

        protected bool IsTrackLocked()
        {
            if (!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly())
                return true;

            return targets.Any(track => ((TrackAsset)track).lockedInHierarchy);
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(IsTrackLocked()))
            {
                DrawInspector();
            }
        }

        internal override bool IsEnabled()
        {
            return TimelineUtility.IsCurrentSequenceValid() && !IsCurrentSequenceReadOnly() && base.IsEnabled();
        }

        internal override void OnHeaderTitleGUI(Rect titleRect, string header)
        {
            serializedObject.Update();

            var textFieldRect = titleRect;
            using (new GUIMixedValueScope(m_Name.hasMultipleDifferentValues))
            {
                var seqWindow = TimelineWindow.instance;

                if (IsTrackLocked())
                {
                    base.OnHeaderTitleGUI(titleRect, m_Name.stringValue);
                }
                else
                {
                    EditorGUI.BeginChangeCheck();
                    string newName = EditorGUI.DelayedTextField(textFieldRect, m_Name.stringValue, EditorStyles.textField);
                    if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(newName))
                    {
                        for (int c = 0; c < targets.Length; c++)
                        {
                            ObjectNames.SetNameSmart(targets[c], newName);
                        }

                        if (seqWindow != null)
                            seqWindow.Repaint();
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        internal override void OnHeaderIconGUI(Rect iconRect)
        {
            if (TimelineWindow.instance == null)
                return;
            using (new EditorGUI.DisabledScope(IsTrackLocked()))
            {
                if (m_HeaderIcon != null)
                    GUI.Label(iconRect, GUIContent.Temp(m_HeaderIcon));
            }
        }

        internal override Rect DrawHeaderHelpAndSettingsGUI(Rect r)
        {
            using (new EditorGUI.DisabledScope(IsTrackLocked()))
            {
                var helpSize = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
                const int kTopMargin = 5;

                // Show Editor Header Items.
                return EditorGUIUtility.DrawEditorHeaderItems(new Rect(r.xMax - helpSize.x, r.y + kTopMargin, helpSize.x, helpSize.y), targets);
            }
        }

        public virtual void OnEnable()
        {
            m_IsBuiltInType = target != null && target.GetType().Assembly == typeof(TrackAsset).Assembly;
            m_Name = serializedObject.FindProperty("m_Name");
            m_TrackCurvesWrapper = new TrackCurvesWrapper(target as TrackAsset);
            m_HeaderIcon = TrackResourceCache.s_DefaultIcon.image;

            // only worry about the first track. if types are different, a different inspector is used.
            var track = target as TrackAsset;
            if (track != null)
            {
                var drawer = CustomTimelineEditorCache.GetTrackEditor(track);
                UnityEngine.Object binding = null;
                var director = m_Context as PlayableDirector;
                if (director != null)
                    binding = director.GetGenericBinding(track);

                var options = drawer.GetTrackOptions(track, binding);
                if (options.icon != null)
                    m_HeaderIcon = options.icon;
                else
                    m_HeaderIcon = TrackResourceCache.GetTrackIcon(track).image;
            }
        }

        void DrawInspector()
        {
            if (serializedObject == null)
                return;

            CurvesOwnerInspectorHelper.PreparePlayableAsset(m_TrackCurvesWrapper);
            serializedObject.Update();

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                DrawTrackProperties();

                if (changeScope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    ApplyChanges();
                }
            }
        }

        protected virtual void DrawTrackProperties()
        {
            var property = serializedObject.GetIterator();
            var expanded = true;
            while (property.NextVisible(expanded))
            {
                // Don't draw script field for built-in types
                if (m_IsBuiltInType && "m_Script" == property.propertyPath)
                    continue;

                EditorGUILayout.PropertyField(property, !expanded);
                expanded = false;
            }
        }

        protected virtual void ApplyChanges()
        {
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        static bool IsCurrentSequenceReadOnly()
        {
            return TimelineWindow.instance.state.editSequence.isReadOnly;
        }
    }
}

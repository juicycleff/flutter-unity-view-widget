using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;
using UnityObject = UnityEngine.Object;

namespace UnityEditor.Timeline
{
    [CustomEditor(typeof(EditorClip)), CanEditMultipleObjects]
    class ClipInspector : Editor
    {
        internal static class Styles
        {
            public static readonly GUIContent StartName = EditorGUIUtility.TrTextContent("Start", "The start time of the clip");
            public static readonly GUIContent DurationName = EditorGUIUtility.TrTextContent("Duration", "The length of the clip");
            public static readonly GUIContent EndName = EditorGUIUtility.TrTextContent("End", "The end time of the clip");
            public static readonly GUIContent EaseInDurationName = EditorGUIUtility.TrTextContent("Ease In Duration", "The length of the blend in");
            public static readonly GUIContent EaseOutDurationName = EditorGUIUtility.TrTextContent("Ease Out Duration", "The length of the blend out");
            public static readonly GUIContent ClipInName = EditorGUIUtility.TrTextContent("Clip In", "Start the clip at this local time");
            public static readonly GUIContent TimeScaleName = EditorGUIUtility.TrTextContent("Speed Multiplier", "Time scale of the playback speed");
            public static readonly GUIContent PreExtrapolateLabel = EditorGUIUtility.TrTextContent("Pre-Extrapolate", "Extrapolation used prior to the first clip");
            public static readonly GUIContent PostExtrapolateLabel = EditorGUIUtility.TrTextContent("Post-Extrapolate", "Extrapolation used after a clip ends");
            public static readonly GUIContent BlendInCurveName = EditorGUIUtility.TrTextContent("In", "Blend In Curve");
            public static readonly GUIContent BlendOutCurveName = EditorGUIUtility.TrTextContent("Out", "Blend Out Curve");
            public static readonly GUIContent PreviewTitle = EditorGUIUtility.TrTextContent("Curve Editor");
            public static readonly GUIContent ClipTimingTitle = EditorGUIUtility.TrTextContent("Clip Timing");
            public static readonly GUIContent AnimationExtrapolationTitle = EditorGUIUtility.TrTextContent("Animation Extrapolation");
            public static readonly GUIContent BlendCurvesTitle = EditorGUIUtility.TrTextContent("Blend Curves");
            public static readonly GUIContent GroupTimingTitle = EditorGUIUtility.TrTextContent("Multiple Clip Timing");
            public static readonly GUIContent MultipleClipsSelectedIncompatibleCapabilitiesWarning = EditorGUIUtility.TrTextContent("Multiple clips selected. Only common properties are shown.");
            public static readonly GUIContent MultipleSelectionTitle = EditorGUIUtility.TrTextContent("Timeline Clips");
            public static readonly GUIContent MultipleClipStartName = EditorGUIUtility.TrTextContent("Start", "The start time of the clip group");
            public static readonly GUIContent MultipleClipEndName = EditorGUIUtility.TrTextContent("End", "The end time of the clip group");
            public static readonly GUIContent TimelineClipFG = DirectorStyles.IconContent("TimelineClipFG");
            public static readonly GUIContent TimelineClipBG = DirectorStyles.IconContent("TimelineClipBG");
        }

        class EditorClipSelection : ICurvesOwnerInspectorWrapper
        {
            public EditorClip editorClip { get; }

            public TimelineClip clip
            {
                get { return editorClip == null ? null : editorClip.clip; }
            }

            public SerializedObject serializedPlayableAsset { get; }

            public ICurvesOwner curvesOwner
            {
                get { return clip; }
            }

            public int lastCurveVersion { get; set; }
            public double lastEvalTime { get; set; }

            public EditorClipSelection(EditorClip anEditorClip)
            {
                editorClip = anEditorClip;
                lastCurveVersion = -1;
                lastEvalTime = -1;

                var so = new SerializedObject(editorClip);
                var playableAssetProperty = so.FindProperty("m_Clip.m_Asset");
                if (playableAssetProperty != null)
                {
                    var asset = playableAssetProperty.objectReferenceValue as UnityEngine.Playables.PlayableAsset;
                    if (asset != null)
                        serializedPlayableAsset = new SerializedObject(asset);
                }
            }

            public double ToLocalTime(double time)
            {
                return clip == null ? time : clip.ToLocalTime(time);
            }
        }

        SerializedProperty m_DisplayNameProperty;
        SerializedProperty m_BlendInDurationProperty;
        SerializedProperty m_BlendOutDurationProperty;
        SerializedProperty m_EaseInDurationProperty;
        SerializedProperty m_EaseOutDurationProperty;
        SerializedProperty m_ClipInProperty;
        SerializedProperty m_TimeScaleProperty;
        SerializedProperty m_PostExtrapolationModeProperty;
        SerializedProperty m_PreExtrapolationModeProperty;
        SerializedProperty m_PostExtrapolationTimeProperty;
        SerializedProperty m_PreExtrapolationTimeProperty;
        SerializedProperty m_MixInCurveProperty;
        SerializedProperty m_MixOutCurveProperty;
        SerializedProperty m_BlendInCurveModeProperty;
        SerializedProperty m_BlendOutCurveModeProperty;

        void InitializeProperties()
        {
            m_DisplayNameProperty = serializedObject.FindProperty("m_Clip.m_DisplayName");
            m_BlendInDurationProperty = serializedObject.FindProperty("m_Clip.m_BlendInDuration");
            m_BlendOutDurationProperty = serializedObject.FindProperty("m_Clip.m_BlendOutDuration");
            m_EaseInDurationProperty = serializedObject.FindProperty("m_Clip.m_EaseInDuration");
            m_EaseOutDurationProperty = serializedObject.FindProperty("m_Clip.m_EaseOutDuration");
            m_ClipInProperty = serializedObject.FindProperty("m_Clip.m_ClipIn");
            m_TimeScaleProperty = serializedObject.FindProperty("m_Clip.m_TimeScale");
            m_PostExtrapolationModeProperty = serializedObject.FindProperty("m_Clip.m_PostExtrapolationMode");
            m_PreExtrapolationModeProperty = serializedObject.FindProperty("m_Clip.m_PreExtrapolationMode");
            m_PostExtrapolationTimeProperty = serializedObject.FindProperty("m_Clip.m_PostExtrapolationTime");
            m_PreExtrapolationTimeProperty = serializedObject.FindProperty("m_Clip.m_PreExtrapolationTime");
            m_MixInCurveProperty = serializedObject.FindProperty("m_Clip.m_MixInCurve");
            m_MixOutCurveProperty = serializedObject.FindProperty("m_Clip.m_MixOutCurve");
            m_BlendInCurveModeProperty = serializedObject.FindProperty("m_Clip.m_BlendInCurveMode");
            m_BlendOutCurveModeProperty = serializedObject.FindProperty("m_Clip.m_BlendOutCurveMode");
        }

        TimelineAsset m_TimelineAsset;

        List<EditorClipSelection> m_SelectionCache;
        Editor m_SelectedPlayableAssetsInspector;

        ClipInspectorCurveEditor m_ClipCurveEditor;
        AnimationCurve[] m_PreviewCurves; // the curves we are currently previewing.
        CurvePresetLibrary m_CurvePresets;

        bool m_IsClipAssetInspectorExpanded = true;
        GUIContent m_ClipAssetTitle = new GUIContent();
        string m_MultiselectionHeaderTitle;

        ClipInspectorSelectionInfo m_SelectionInfo;

        const double k_TimeScaleSensitivity = 0.003;

        bool hasMultipleSelection
        {
            get { return targets.Length > 1; }
        }

        float currentFrameRate
        {
            get { return m_TimelineAsset != null ? m_TimelineAsset.editorSettings.fps : TimelineAsset.EditorSettings.kDefaultFps; }
        }

        bool selectionHasIncompatibleCapabilities
        {
            get
            {
                return !(m_SelectionInfo.supportsBlending
                    && m_SelectionInfo.supportsClipIn
                    && m_SelectionInfo.supportsExtrapolation
                    && m_SelectionInfo.supportsSpeedMultiplier);
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return base.RequiresConstantRepaint() || (m_SelectedPlayableAssetsInspector != null && m_SelectedPlayableAssetsInspector.RequiresConstantRepaint());
        }

        internal override void OnHeaderTitleGUI(Rect titleRect, string header)
        {
            if (hasMultipleSelection)
            {
                base.OnHeaderTitleGUI(titleRect, m_MultiselectionHeaderTitle);
                return;
            }

            if (m_DisplayNameProperty != null)
            {
                using (new EditorGUI.DisabledScope(!IsEnabled()))
                {
                    serializedObject.Update();
                    if (IsLocked())
                    {
                        base.OnHeaderTitleGUI(titleRect, m_DisplayNameProperty.stringValue);
                    }
                    else
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.DelayedTextField(titleRect, m_DisplayNameProperty, GUIContent.none);
                        if (EditorGUI.EndChangeCheck())
                        {
                            ApplyModifiedProperties();
                            TimelineWindow.RepaintIfEditingTimelineAsset(m_TimelineAsset);
                        }
                    }
                }
            }
        }

        internal override Rect DrawHeaderHelpAndSettingsGUI(Rect r)
        {
            using (new EditorGUI.DisabledScope(IsLocked()))
            {
                var helpSize = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
                const int kTopMargin = 5;
                // Show Editor Header Items.
                return EditorGUIUtility.DrawEditorHeaderItems(new Rect(r.xMax - helpSize.x, r.y + kTopMargin, helpSize.x, helpSize.y), targets);
            }
        }

        internal override void OnHeaderIconGUI(Rect iconRect)
        {
            using (new EditorGUI.DisabledScope(IsLocked()))
            {
                var bgColor = Color.white;
                if (!EditorGUIUtility.isProSkin)
                    bgColor.a = 0.55f;
                using (new GUIColorOverride(bgColor))
                {
                    GUI.Label(iconRect, Styles.TimelineClipBG);
                }

                var fgColor = Color.white;
                if (m_SelectionInfo != null && m_SelectionInfo.uniqueParentTracks.Count == 1)
                    fgColor = TrackResourceCache.GetTrackColor(m_SelectionInfo.uniqueParentTracks.First());

                using (new GUIColorOverride(fgColor))
                {
                    GUI.Label(iconRect, Styles.TimelineClipFG);
                }
            }
        }

        public void OnEnable()
        {
            m_ClipCurveEditor = new ClipInspectorCurveEditor();

            m_SelectionCache = new List<EditorClipSelection>();
            var selectedClips = new List<TimelineClip>();
            foreach (var editorClipObject in targets)
            {
                var editorClip = editorClipObject as EditorClip;
                if (editorClip != null)
                {
                    //all selected clips should have the same TimelineAsset
                    if (!IsTimelineAssetValidForEditorClip(editorClip))
                    {
                        m_SelectionCache.Clear();
                        return;
                    }
                    m_SelectionCache.Add(new EditorClipSelection(editorClip));
                    selectedClips.Add(editorClip.clip);
                }
            }

            InitializeProperties();
            m_SelectionInfo = new ClipInspectorSelectionInfo(selectedClips);

            if (m_SelectionInfo.selectedAssetTypesAreHomogeneous)
            {
                var selectedAssets = m_SelectionCache.Select(e => e.clip.asset).ToArray();
                m_SelectedPlayableAssetsInspector = TimelineInspectorUtility.GetInspectorForObjects(selectedAssets);
            }

            m_MultiselectionHeaderTitle = m_SelectionCache.Count + " " + Styles.MultipleSelectionTitle.text;
            m_ClipAssetTitle.text = PlayableAssetSectionTitle();
        }

        void DrawClipProperties()
        {
            var dirtyEditorClipSelection = m_SelectionCache.Where(s => s.editorClip.GetHashCode() != s.editorClip.lastHash);
            UnselectCurves();

            EditorGUI.BeginChangeCheck();

            //Group Selection
            if (hasMultipleSelection)
            {
                GUILayout.Label(Styles.GroupTimingTitle);
                EditorGUI.indentLevel++;
                DrawGroupSelectionProperties();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            //Draw clip timing
            GUILayout.Label(Styles.ClipTimingTitle);

            if (hasMultipleSelection && selectionHasIncompatibleCapabilities)
            {
                GUILayout.Label(Styles.MultipleClipsSelectedIncompatibleCapabilitiesWarning, EditorStyles.helpBox);
            }

            EditorGUI.indentLevel++;

            if (!m_SelectionInfo.containsAtLeastTwoClipsOnTheSameTrack)
            {
                DrawStartTimeField();
                DrawEndTimeField();
            }

            if (!hasMultipleSelection)
            {
                DrawDurationProperty();
            }

            if (m_SelectionInfo.supportsBlending)
            {
                EditorGUILayout.Space();
                DrawBlendingProperties();
            }

            if (m_SelectionInfo.supportsClipIn)
            {
                EditorGUILayout.Space();
                DrawClipInProperty();
            }

            if (!hasMultipleSelection && m_SelectionInfo.supportsSpeedMultiplier)
            {
                EditorGUILayout.Space();
                DrawTimeScale();
            }

            EditorGUI.indentLevel--;

            bool hasDirtyEditorClips = false;
            foreach (var editorClipSelection in dirtyEditorClipSelection)
            {
                EditorUtility.SetDirty(editorClipSelection.editorClip);
                hasDirtyEditorClips = true;
            }

            //Re-evaluate the graph in case of a change in properties
            bool propertiesHaveChanged = false;
            if (EditorGUI.EndChangeCheck() || hasDirtyEditorClips)
            {
                if (TimelineWindow.IsEditingTimelineAsset(m_TimelineAsset) && TimelineWindow.instance.state != null)
                {
                    TimelineWindow.instance.state.Evaluate();
                    TimelineWindow.instance.Repaint();
                }
                propertiesHaveChanged = true;
            }

            //Draw Animation Extrapolation
            if (m_SelectionInfo.supportsExtrapolation)
            {
                EditorGUILayout.Space();
                GUILayout.Label(Styles.AnimationExtrapolationTitle);
                EditorGUI.indentLevel++;
                DrawExtrapolationOptions();
                EditorGUI.indentLevel--;
            }

            //Blend curves
            if (m_SelectionInfo.supportsBlending)
            {
                EditorGUILayout.Space();
                GUILayout.Label(Styles.BlendCurvesTitle);
                EditorGUI.indentLevel++;
                DrawBlendOptions();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            if (CanShowPlayableAssetInspector())
            {
                DrawClipAssetGui();
            }

            if (propertiesHaveChanged)
            {
                foreach (var item in m_SelectionCache)
                    item.editorClip.lastHash = item.editorClip.GetHashCode();
                m_SelectionInfo.Update();
            }
        }

        public override void OnInspectorGUI()
        {
            if (TimelineWindow.instance == null || m_TimelineAsset == null)
                return;

            using (new EditorGUI.DisabledScope(IsLocked()))
            {
                EditMode.HandleModeClutch();

                serializedObject.Update();
                DrawClipProperties();
                ApplyModifiedProperties();
            }
        }

        internal override bool IsEnabled()
        {
            if (!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly())
                return false;

            if (m_TimelineAsset != TimelineWindow.instance.state.editSequence.asset)
                return false;
            return base.IsEnabled();
        }

        void DrawTimeScale()
        {
            var inputEvent = InputEvent.None;
            var newEndTime = m_SelectionInfo.end;
            var oldTimeScale = m_TimeScaleProperty.doubleValue;

            EditorGUI.BeginChangeCheck();
            var newTimeScale = TimelineInspectorUtility.DelayedAndDraggableDoubleField(Styles.TimeScaleName, oldTimeScale, ref inputEvent, k_TimeScaleSensitivity);

            if (EditorGUI.EndChangeCheck())
            {
                newTimeScale = newTimeScale.Clamp(TimelineClip.kTimeScaleMin, TimelineClip.kTimeScaleMax);
                newEndTime = m_SelectionInfo.start + (m_SelectionInfo.duration * oldTimeScale / newTimeScale);
            }
            EditMode.inputHandler.ProcessTrim(inputEvent, newEndTime, true);
        }

        void DrawStartTimeField()
        {
            var inputEvent = InputEvent.None;
            var newStart = TimelineInspectorUtility.TimeFieldUsingTimeReference(Styles.StartName, m_SelectionInfo.multipleClipStart, false, m_SelectionInfo.hasMultipleStartValues, currentFrameRate, 0.0, TimelineClip.kMaxTimeValue, ref inputEvent);

            if (inputEvent.InputHasBegun() && m_SelectionInfo.hasMultipleStartValues)
            {
                var items = ItemsUtils.ToItems(m_SelectionInfo.clips);
                EditMode.inputHandler.SetValueForEdge(items, AttractedEdge.Left, newStart); //if the field has multiple values, set the same start on all selected clips
                m_SelectionInfo.Update(); //clips could have moved relative to each other, recalculate
            }

            EditMode.inputHandler.ProcessMove(inputEvent, newStart);
        }

        void DrawEndTimeField()
        {
            var inputEvent = InputEvent.None;
            var newEndTime = TimelineInspectorUtility.TimeFieldUsingTimeReference(Styles.EndName, m_SelectionInfo.multipleClipEnd, false, m_SelectionInfo.hasMultipleEndValues, currentFrameRate, 0, TimelineClip.kMaxTimeValue, ref inputEvent);

            if (inputEvent.InputHasBegun() && m_SelectionInfo.hasMultipleEndValues)
            {
                var items = ItemsUtils.ToItems(m_SelectionInfo.clips);
                EditMode.inputHandler.SetValueForEdge(items, AttractedEdge.Right, newEndTime); //if the field has multiple value, set the same end on all selected clips
                m_SelectionInfo.Update(); //clips could have moved relative to each other, recalculate
            }

            var newStartValue = m_SelectionInfo.multipleClipStart + (newEndTime - m_SelectionInfo.multipleClipEnd);
            EditMode.inputHandler.ProcessMove(inputEvent, newStartValue);
        }

        void DrawClipAssetGui()
        {
            const float labelIndent = 34;
            if (m_SelectedPlayableAssetsInspector == null)
                return;

            var rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar);
            var oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = rect.width - labelIndent;
            m_IsClipAssetInspectorExpanded = EditorGUI.FoldoutTitlebar(rect, m_ClipAssetTitle, m_IsClipAssetInspectorExpanded, false);
            EditorGUIUtility.labelWidth = oldWidth;
            if (m_IsClipAssetInspectorExpanded)
            {
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                ShowPlayableAssetInspector();
                EditorGUI.indentLevel--;
            }
        }

        void DrawExtrapolationOptions()
        {
            EditorGUI.BeginChangeCheck();

            // PreExtrapolation
            var preExtrapolationTime =  m_PreExtrapolationTimeProperty.doubleValue;
            bool hasPreExtrap = preExtrapolationTime > 0.0;
            if (hasPreExtrap)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PreExtrapolationModeProperty, Styles.PreExtrapolateLabel);
                using (new GUIMixedValueScope(m_PreExtrapolationTimeProperty.hasMultipleDifferentValues))
                    EditorGUILayout.DoubleField(preExtrapolationTime, EditorStyles.label);
                EditorGUILayout.EndHorizontal();
            }

            // PostExtrapolation
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PostExtrapolationModeProperty, Styles.PostExtrapolateLabel);
                using (new GUIMixedValueScope(m_PostExtrapolationTimeProperty.hasMultipleDifferentValues))
                    EditorGUILayout.DoubleField(m_PostExtrapolationTimeProperty.doubleValue, EditorStyles.label);
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                // if these options change the interval tree may need to be rebuilt.
                if (TimelineWindow.IsEditingTimelineAsset(m_TimelineAsset) && TimelineWindow.instance.state != null)
                {
                    TimelineWindow.instance.state.Refresh();
                }
            }
        }

        void OnDestroy()
        {
            DestroyImmediate(m_SelectedPlayableAssetsInspector);
        }

        public override GUIContent GetPreviewTitle()
        {
            return Styles.PreviewTitle;
        }

        public override bool HasPreviewGUI()
        {
            return m_PreviewCurves != null;
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            if (m_PreviewCurves != null && m_ClipCurveEditor != null)
            {
                SetCurveEditorTrackHead();
                m_ClipCurveEditor.OnGUI(r, m_CurvePresets);
            }
        }

        void SetCurveEditorTrackHead()
        {
            if (TimelineWindow.instance == null || TimelineWindow.instance.state == null)
                return;

            if (hasMultipleSelection)
                return;

            var editorClip = target as EditorClip;
            if (editorClip == null)
                return;

            var director = TimelineWindow.instance.state.editSequence.director;

            if (director == null)
                return;

            m_ClipCurveEditor.trackTime = ClipInspectorCurveEditor.kDisableTrackTime;
        }

        void UnselectCurves()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                m_PreviewCurves = null;
                if (m_ClipCurveEditor != null)
                    m_ClipCurveEditor.SetUpdateCurveCallback(null);
            }
        }

        // Callback when the mixin/mixout properties are clicked on
        void OnMixCurveSelected(string title, CurvePresetLibrary library, SerializedProperty curveSelected, bool easeIn)
        {
            m_CurvePresets = library;
            m_PreviewCurves = new[] { curveSelected.animationCurveValue  };
            m_ClipCurveEditor.headerString = title;
            m_ClipCurveEditor.SetCurves(m_PreviewCurves, null);
            m_ClipCurveEditor.SetSelected(curveSelected.animationCurveValue);
            if (easeIn)
                m_ClipCurveEditor.SetUpdateCurveCallback(MixInCurveUpdated);
            else
                m_ClipCurveEditor.SetUpdateCurveCallback(MixOutCurveUpdated);
            Repaint();
        }

        // callback when the mix property is updated
        void MixInCurveUpdated(AnimationCurve curve, EditorCurveBinding binding)
        {
            curve.keys = CurveEditUtility.SanitizeCurveKeys(curve.keys, true);
            m_MixInCurveProperty.animationCurveValue = curve;
            serializedObject.ApplyModifiedProperties();
            var editorClip = target as EditorClip;
            if (editorClip != null)
                editorClip.lastHash = editorClip.GetHashCode();
            RefreshCurves();
        }

        void MixOutCurveUpdated(AnimationCurve curve, EditorCurveBinding binding)
        {
            curve.keys = CurveEditUtility.SanitizeCurveKeys(curve.keys, false);
            m_MixOutCurveProperty.animationCurveValue = curve;
            serializedObject.ApplyModifiedProperties();
            var editorClip = target as EditorClip;
            if (editorClip != null)
                editorClip.lastHash = editorClip.GetHashCode();
            RefreshCurves();
        }

        void RefreshCurves()
        {
            AnimationCurvePreviewCache.ClearCache();
            TimelineWindow.RepaintIfEditingTimelineAsset(m_TimelineAsset);
            Repaint();
        }

        void DrawBlendCurve(GUIContent title, SerializedProperty modeProperty, SerializedProperty curveProperty, Action<SerializedProperty> onCurveClick)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(modeProperty, title);
            if (hasMultipleSelection)
            {
                GUILayout.FlexibleSpace();
            }
            else
            {
                using (new EditorGUI.DisabledScope(modeProperty.intValue != (int)TimelineClip.BlendCurveMode.Manual))
                {
                    ClipInspectorCurveEditor.CurveField(GUIContent.none, curveProperty, onCurveClick);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        void ShowPlayableAssetInspector()
        {
            if (!m_SelectionInfo.selectedAssetTypesAreHomogeneous)
                return;

            if (m_SelectedPlayableAssetsInspector != null)
            {
                foreach (var selectedItem in m_SelectionCache)
                    CurvesOwnerInspectorHelper.PreparePlayableAsset(selectedItem);

                EditorGUI.BeginChangeCheck();
                using (new EditorGUI.DisabledScope(IsLocked()))
                {
                    m_SelectedPlayableAssetsInspector.OnInspectorGUI();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    MarkClipsDirty();
                    if (TimelineWindow.IsEditingTimelineAsset(m_TimelineAsset) && TimelineWindow.instance.state != null)
                    {
                        var basicInspector = m_SelectedPlayableAssetsInspector as BasicAssetInspector;
                        if (basicInspector != null)
                            basicInspector.ApplyChanges();
                        else
                            TimelineEditor.Refresh(RefreshReason.ContentsModified);
                    }
                }
            }
        }

        void ApplyModifiedProperties()
        {
            // case 926861 - we need to force the track to be dirty since modifying the clip does not
            //  automatically mark the track asset as dirty
            if (serializedObject.ApplyModifiedProperties())
            {
                foreach (var obj in serializedObject.targetObjects)
                {
                    var editorClip = obj as EditorClip;
                    if (editorClip != null && editorClip.clip != null && editorClip.clip.parentTrack != null)
                    {
                        editorClip.clip.MarkDirty();
                        EditorUtility.SetDirty(editorClip.clip.parentTrack);
                    }
                }
            }
        }

        void MarkClipsDirty()
        {
            foreach (var obj in targets)
            {
                var editorClip = obj as EditorClip;
                if (editorClip != null && editorClip.clip != null)
                {
                    editorClip.clip.MarkDirty();
                }
            }
        }

        string PlayableAssetSectionTitle()
        {
            var firstSelectedClipAsset = m_SelectionCache.Any() ? m_SelectionCache.First().clip.asset : null;
            return firstSelectedClipAsset != null
                ? ObjectNames.NicifyVariableName(firstSelectedClipAsset.GetType().Name)
                : string.Empty;
        }

        bool IsTimelineAssetValidForEditorClip(EditorClip editorClip)
        {
            var trackAsset = editorClip.clip.parentTrack;
            if (trackAsset == null)
                return false;

            var clipTimelineAsset = trackAsset.timelineAsset;
            if (m_TimelineAsset == null)
                m_TimelineAsset = clipTimelineAsset;
            else if (clipTimelineAsset != m_TimelineAsset)
            {
                m_TimelineAsset = null;
                return false;
            }
            return true;
        }

        bool CanShowPlayableAssetInspector()
        {
            if (hasMultipleSelection)
                return m_SelectedPlayableAssetsInspector != null &&
                    m_SelectedPlayableAssetsInspector.canEditMultipleObjects &&
                    m_SelectionInfo.selectedAssetTypesAreHomogeneous;
            else
                return true;
        }

        void DrawDurationProperty()
        {
            var minDuration = 1.0 / 30.0;
            if (currentFrameRate > float.Epsilon)
            {
                minDuration = 1.0 / currentFrameRate;
            }

            var inputEvent = InputEvent.None;
            var newDuration = TimelineInspectorUtility.DurationFieldUsingTimeReference(
                Styles.DurationName, m_SelectionInfo.start, m_SelectionInfo.end, false, m_SelectionInfo.hasMultipleDurationValues, currentFrameRate, minDuration, TimelineClip.kMaxTimeValue, ref inputEvent);
            EditMode.inputHandler.ProcessTrim(inputEvent, m_SelectionInfo.start + newDuration, false);
        }

        void DrawBlendingProperties()
        {
            var useBlendIn = m_SelectionInfo.hasBlendIn;
            var maxBlendDuration = m_SelectionInfo.smallestDuration * 0.49;
            var blendMax = useBlendIn ? TimelineClip.kMaxTimeValue : maxBlendDuration;
            var inputEvent = InputEvent.None;

            TimelineInspectorUtility.TimeField(useBlendIn
                ? m_BlendInDurationProperty
                : m_EaseInDurationProperty, Styles.EaseInDurationName, useBlendIn, currentFrameRate, 0, blendMax, ref inputEvent);

            var useBlendOut = m_SelectionInfo.hasBlendOut;
            blendMax = useBlendOut ? TimelineClip.kMaxTimeValue : maxBlendDuration;
            TimelineInspectorUtility.TimeField(useBlendOut
                ? m_BlendOutDurationProperty
                : m_EaseOutDurationProperty, Styles.EaseOutDurationName, useBlendOut, currentFrameRate, 0, blendMax, ref inputEvent);
        }

        void DrawClipInProperty()
        {
            var action = InputEvent.None;
            TimelineInspectorUtility.TimeField(m_ClipInProperty, Styles.ClipInName, false, currentFrameRate, 0, TimelineClip.kMaxTimeValue, ref action);
        }

        void DrawBlendOptions()
        {
            EditorGUI.BeginChangeCheck();

            DrawBlendCurve(Styles.BlendInCurveName, m_BlendInCurveModeProperty, m_MixInCurveProperty, x => OnMixCurveSelected("Blend In", BuiltInPresets.blendInPresets, x, true));
            DrawBlendCurve(Styles.BlendOutCurveName, m_BlendOutCurveModeProperty, m_MixOutCurveProperty, x => OnMixCurveSelected("Blend Out", BuiltInPresets.blendOutPresets, x, false));

            if (EditorGUI.EndChangeCheck())
                TimelineWindow.RepaintIfEditingTimelineAsset(m_TimelineAsset);
        }

        void DrawGroupSelectionProperties()
        {
            var inputEvent = InputEvent.None;
            var newStartTime = TimelineInspectorUtility.TimeField(Styles.MultipleClipStartName, m_SelectionInfo.multipleClipStart, false, false, currentFrameRate, 0, TimelineClip.kMaxTimeValue, ref inputEvent);
            EditMode.inputHandler.ProcessMove(inputEvent, newStartTime);

            inputEvent = InputEvent.None;
            var newEndTime = TimelineInspectorUtility.TimeField(Styles.MultipleClipEndName, m_SelectionInfo.multipleClipEnd, false, false, currentFrameRate, 0, TimelineClip.kMaxTimeValue, ref inputEvent);
            var newStartValue = newStartTime + (newEndTime - m_SelectionInfo.multipleClipEnd);
            EditMode.inputHandler.ProcessMove(inputEvent, newStartValue);
        }

        bool IsLocked()
        {
            if (!TimelineUtility.IsCurrentSequenceValid() || IsCurrentSequenceReadOnly())
                return true;

            return targets.OfType<EditorClip>().Any(t => t.clip.parentTrack != null && t.clip.parentTrack.lockedInHierarchy);
        }

        static bool IsCurrentSequenceReadOnly()
        {
            return TimelineWindow.instance.state.editSequence.isReadOnly;
        }
    }
}

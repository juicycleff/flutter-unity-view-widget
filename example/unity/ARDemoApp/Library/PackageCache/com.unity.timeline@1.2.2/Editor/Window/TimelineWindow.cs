using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    [EditorWindowTitle(title = "Timeline", useTypeNameAsIconName = true)]
    partial class TimelineWindow : EditorWindow, IHasCustomMenu
    {
        [Serializable]
        public class TimelineWindowPreferences
        {
            public bool frameSnap = true;
            public bool edgeSnaps = true;
            public bool muteAudioScrub = true;
            public bool playRangeLoopMode = true;
            public PlaybackScrollMode autoScrollMode;
            public EditMode.EditType editType = EditMode.EditType.Mix;
            public TimeReferenceMode timeReferenceMode = TimeReferenceMode.Local;
        }

        [SerializeField] TimelineWindowPreferences m_Preferences = new TimelineWindowPreferences();
        public TimelineWindowPreferences preferences { get { return m_Preferences; } }

        [SerializeField]
        EditorGUIUtility.EditorLockTracker m_LockTracker = new EditorGUIUtility.EditorLockTracker();

        readonly PreviewResizer m_PreviewResizer = new PreviewResizer();
        bool m_LastFrameHadSequence;
        bool m_ForceRefreshLastSelection;
        int m_CurrentSceneHashCode = -1;

        [NonSerialized]
        bool m_HasBeenInitialized;

        [SerializeField]
        SequenceHierarchy m_SequenceHierarchy;
        static SequenceHierarchy s_LastHierarchy;

        public static TimelineWindow instance { get; private set; }
        public Rect clientArea { get; set; }
        public bool isDragging { get; set; }
        public static DirectorStyles styles { get { return DirectorStyles.Instance; } }
        public List<TimelineTrackBaseGUI> allTracks
        {
            get
            {
                return treeView != null ? treeView.allTrackGuis : new List<TimelineTrackBaseGUI>();
            }
        }

        public WindowState state { get; private set; }

        public bool locked
        {
            get
            {
                // we can never be in a locked state if there is no timeline asset
                if (state.editSequence.asset == null)
                    return false;

                return m_LockTracker.isLocked;
            }
            set { m_LockTracker.isLocked = value; }
        }

        public bool hierarchyChangedThisFrame { get; private set; }

        public TimelineWindow()
        {
            InitializeManipulators();
            m_LockTracker.lockStateChanged.AddPersistentListener(OnLockStateChanged, UnityEventCallState.EditorAndRuntime);
        }

        void OnLockStateChanged(bool locked)
        {
            // Make sure that upon unlocking, any selection change is updated
            // Case 1123119 -- only force rebuild if not recording
            if (!locked)
                RefreshSelection(state != null && !state.recording);
        }

        void OnEnable()
        {
            if (m_SequencePath == null)
                m_SequencePath = new SequencePath();

            if (m_SequenceHierarchy == null)
            {
                // The sequence hierarchy will become null if maximize on play is used for in/out of playmode
                // a static var will hang on to the reference
                if (s_LastHierarchy != null)
                    m_SequenceHierarchy = s_LastHierarchy;
                else
                    m_SequenceHierarchy = SequenceHierarchy.CreateInstance();

                state = null;
            }
            s_LastHierarchy = m_SequenceHierarchy;

            titleContent = GetLocalizedTitleContent();

            m_PreviewResizer.Init("TimelineWindow");

            // Unmaximize fix : when unmaximizing, a new window is enabled and disabled. Prevent it from overriding the instance pointer.
            if (instance == null)
                instance = this;

            AnimationClipCurveCache.Instance.OnEnable();
            TrackAsset.OnClipPlayableCreate += m_PlayableLookup.UpdatePlayableLookup;
            TrackAsset.OnTrackAnimationPlayableCreate += m_PlayableLookup.UpdatePlayableLookup;

            if (state == null)
            {
                state = new WindowState(this, s_LastHierarchy);
                Initialize();
                RefreshSelection(true);
                m_ForceRefreshLastSelection = true;
            }
        }

        void OnDisable()
        {
            if (instance == this)
                instance = null;

            if (state != null)
                state.Reset();

            if (instance == null)
                SelectionManager.RemoveTimelineSelection();

            AnimationClipCurveCache.Instance.OnDisable();
            TrackAsset.OnClipPlayableCreate -= m_PlayableLookup.UpdatePlayableLookup;
            TrackAsset.OnTrackAnimationPlayableCreate -= m_PlayableLookup.UpdatePlayableLookup;
            TimelineWindowViewPrefs.SaveAll();
            TimelineWindowViewPrefs.UnloadAllViewModels();
        }

        void OnDestroy()
        {
            if (state != null)
            {
                state.OnDestroy();
            }
            m_HasBeenInitialized = false;
            RemoveEditorCallbacks();
            TimelineAnimationUtilities.UnlinkAnimationWindow();
        }

        void OnLostFocus()
        {
            isDragging = false;

            if (state != null)
                state.captured.Clear();

            Repaint();
        }

        void OnFocus()
        {
            if (state == null) return;

            // selection may have changed while Timeline Editor was looking away
            RefreshSelection(false);

            // Inline curves may have become out of sync
            RefreshInlineCurves();
        }

        void OnHierarchyChange()
        {
            hierarchyChangedThisFrame = true;
            Repaint();
        }

        void OnStateChange()
        {
            state.UpdateRecordingState();
            if (treeView != null && state.editSequence.asset != null)
                treeView.Reload();
            if (m_MarkerHeaderGUI != null)
                m_MarkerHeaderGUI.Rebuild();
        }

        void OnGUI()
        {
            InitializeGUIIfRequired();
            UpdateGUIConstants();
            UpdateViewStateHash();

            EditMode.HandleModeClutch(); // TODO We Want that here?

            DetectStylesChange();
            DetectActiveSceneChanges();
            DetectStateChanges();

            state.ProcessStartFramePendingUpdates();

            var clipRect = new Rect(0.0f, 0.0f, position.width, position.height);
            clipRect.xMin += state.sequencerHeaderWidth;

            using (new GUIViewportScope(clipRect))
                state.InvokeWindowOnGuiStarted(Event.current);

            if (Event.current.type == EventType.MouseDrag && state != null && state.mouseDragLag > 0.0f)
            {
                state.mouseDragLag -= Time.deltaTime;
                return;
            }

            if (PerformUndo())
                return;

            if (EditorApplication.isPlaying)
            {
                if (state != null)
                {
                    if (state.recording)
                        state.recording = false;
                }
                Repaint();
            }

            clientArea = position;

            PlaybackScroller.AutoScroll(state);
            DoLayout();

            // overlays
            if (state.captured.Count > 0)
            {
                using (new GUIViewportScope(clipRect))
                {
                    foreach (var o in state.captured)
                    {
                        o.Overlay(Event.current, state);
                    }
                    Repaint();
                }
            }

            if (state.showQuadTree)
                state.spacePartitioner.DebugDraw();

            // attempt another rebuild -- this will avoid 1 frame flashes
            if (Event.current.type == EventType.Repaint)
            {
                RebuildGraphIfNecessary();
                state.ProcessEndFramePendingUpdates();
            }

            using (new GUIViewportScope(clipRect))
            {
                if (Event.current.type == EventType.Repaint)
                    EditMode.inputHandler.OnGUI(state, Event.current);
            }

            if (Event.current.type == EventType.Repaint)
                hierarchyChangedThisFrame = false;
        }

        static void DetectStylesChange()
        {
            DirectorStyles.ReloadStylesIfNeeded();
        }

        void DetectActiveSceneChanges()
        {
            if (m_CurrentSceneHashCode == -1)
            {
                m_CurrentSceneHashCode = SceneManager.GetActiveScene().GetHashCode();
            }

            if (m_CurrentSceneHashCode != SceneManager.GetActiveScene().GetHashCode())
            {
                bool isSceneStillLoaded = false;
                for (int a = 0; a < SceneManager.sceneCount; a++)
                {
                    var scene = SceneManager.GetSceneAt(a);
                    if (scene.GetHashCode() == m_CurrentSceneHashCode && scene.isLoaded)
                    {
                        isSceneStillLoaded = true;
                        break;
                    }
                }

                if (!isSceneStillLoaded)
                {
                    if (!locked)
                        ClearCurrentTimeline();
                    m_CurrentSceneHashCode = SceneManager.GetActiveScene().GetHashCode();
                }
            }
        }

        void DetectStateChanges()
        {
            if (state != null)
            {
                state.editSequence.ResetIsReadOnly();   //Force reset readonly for asset flag for each frame.
                // detect if the sequence was removed under our feet
                if (m_LastFrameHadSequence && state.editSequence.asset == null)
                {
                    ClearCurrentTimeline();
                }
                m_LastFrameHadSequence = state.editSequence.asset != null;

                // the currentDirector can get set to null by a deletion or scene unloading so polling is required
                if (state.editSequence.director == null)
                {
                    state.recording = false;
                    state.previewMode = false;

                    if (!locked)
                    {
                        // the user may be adding a new PlayableDirector to a selected GameObject, make sure the timeline editor is shows the proper director if none is already showing
                        var selectedGameObject = Selection.activeObject != null ? Selection.activeObject as GameObject : null;
                        var selectedDirector = selectedGameObject != null ? selectedGameObject.GetComponent<PlayableDirector>() : null;
                        if (selectedDirector != null)
                        {
                            SetCurrentTimeline(selectedDirector);
                        }
                    }
                }
                else
                {
                    // the user may have changed the timeline associated with the current director
                    if (state.editSequence.asset != state.editSequence.director.playableAsset)
                    {
                        if (!locked)
                        {
                            SetCurrentTimeline(state.editSequence.director);
                        }
                        else
                        {
                            // Keep locked on the current timeline but set the current director to null since it's not the timeline owner anymore
                            SetCurrentTimeline(state.editSequence.asset);
                        }
                    }
                }
            }
        }

        void Initialize()
        {
            if (!m_HasBeenInitialized)
            {
                InitializeStateChange();
                InitializeEditorCallbacks();
                m_HasBeenInitialized = true;
            }
        }

        void RefreshLastSelectionIfRequired()
        {
            // case 1088918 - workaround for the instanceID to object cache being update during Awake.
            // This corrects any playableDirector ptrs with the correct cached version
            // This can happen when going from edit to playmode
            if (m_ForceRefreshLastSelection)
            {
                m_ForceRefreshLastSelection = false;
                RestoreLastSelection(true);
            }
        }

        void InitializeGUIIfRequired()
        {
            RefreshLastSelectionIfRequired();
            InitializeTimeArea();
            if (treeView == null && state.editSequence.asset != null)
            {
                treeView = new TimelineTreeViewGUI(this, state.editSequence.asset, position);
            }
        }

        void UpdateGUIConstants()
        {
            m_HorizontalScrollBarSize =
                GUI.skin.horizontalScrollbar.fixedHeight + GUI.skin.horizontalScrollbar.margin.top;
            m_VerticalScrollBarSize = (treeView != null && treeView.showingVerticalScrollBar)
                ? GUI.skin.verticalScrollbar.fixedWidth + GUI.skin.verticalScrollbar.margin.left
                : 0;
        }

        void UpdateViewStateHash()
        {
            if (Event.current.type == EventType.Layout)
                state.UpdateViewStateHash();
        }

        static bool PerformUndo()
        {
            if (!Event.current.isKey)
                return false;

            if (Event.current.keyCode != KeyCode.Z)
                return false;

            if (!EditorGUI.actionKey)
                return false;

            return true;
        }

        public void RebuildGraphIfNecessary(bool evaluate = true)
        {
            if (state == null || state.editSequence.director == null || state.editSequence.asset == null)
                return;

            if (state.rebuildGraph)
            {
                // rebuilding the graph resets the time
                double time = state.editSequence.time;

                var wasPlaying = false;

                // disable preview mode,
                if (!EditorApplication.isPlaying)
                {
                    wasPlaying = state.playing;

                    state.previewMode = false;
                    state.GatherProperties(state.masterSequence.director);
                }
                state.RebuildPlayableGraph();
                state.editSequence.time = time;

                if (wasPlaying)
                    state.Play();

                if (evaluate)
                {
                    // put the scene back in the correct state
                    state.EvaluateImmediate();

                    // this is necessary to see accurate results when inspector refreshes
                    // case 1154802 - this will property re-force time on the director, so
                    //  the play head won't snap back to the timeline duration on rebuilds
                    if (!state.playing)
                        state.Evaluate();
                }
                Repaint();
            }

            state.rebuildGraph = false;
        }

        // for tests
        public new void RepaintImmediately()
        {
            base.RepaintImmediately();
        }

        internal static bool IsEditingTimelineAsset(TimelineAsset timelineAsset)
        {
            return instance != null && instance.state != null && instance.state.editSequence.asset == timelineAsset;
        }

        internal static void RepaintIfEditingTimelineAsset(TimelineAsset timelineAsset)
        {
            if (IsEditingTimelineAsset(timelineAsset))
                instance.Repaint();
        }

        internal class DoCreateTimeline : ProjectWindowCallback.EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var timeline = ScriptableObject.CreateInstance<TimelineAsset>();
                AssetDatabase.CreateAsset(timeline, pathName);
                ProjectWindowUtil.ShowCreatedAsset(timeline);
            }
        }

        [MenuItem("Assets/Create/Timeline", false, 450)]
        public static void CreateNewTimeline()
        {
            var icon = EditorGUIUtility.IconContent("TimelineAsset Icon").image as Texture2D;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateTimeline>(), "New Timeline.playable", icon, null);
        }

        [MenuItem("Window/Sequencing/Timeline", false, 1)]
        public static void ShowWindow()
        {
            GetWindow<TimelineWindow>(typeof(SceneView));
            instance.Focus();
        }

        [OnOpenAsset(1)]
        public static bool OnDoubleClick(int instanceID, int line)
        {
            var assetDoubleClicked = EditorUtility.InstanceIDToObject(instanceID) as TimelineAsset;
            if (assetDoubleClicked == null)
                return false;

            ShowWindow();
            instance.SetCurrentTimeline(assetDoubleClicked);

            return true;
        }

        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            bool disabled = state == null || state.editSequence.asset == null;

            m_LockTracker.AddItemsToMenu(menu, disabled);
        }

        protected virtual void ShowButton(Rect r)
        {
            bool disabled = state == null || state.editSequence.asset == null;

            m_LockTracker.ShowButton(r, DirectorStyles.Instance.lockButton, disabled);
        }

        internal void TreeViewKeyboardCallback()
        {
            if (Event.current.type != EventType.KeyDown)
                return;
            if (TimelineAction.HandleShortcut(state, Event.current))
            {
                Event.current.Use();
            }
        }
    }
}

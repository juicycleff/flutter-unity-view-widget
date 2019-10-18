using System;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Playable that controls and instantiates a Prefab.
    /// </summary>
    public class PrefabControlPlayable : PlayableBehaviour
    {
        GameObject m_Instance;

#if UNITY_EDITOR
        private bool m_IsActiveCached;
#endif

        /// <summary>
        /// Creates a Playable with a PrefabControlPlayable behaviour attached
        /// </summary>
        /// <param name="graph">The PlayableGraph to inject the Playable into.</param>
        /// <param name="prefabGameObject">The prefab to instantiate from</param>
        /// <param name="parentTransform">Transform to parent instance to. Can be null.</param>
        /// <returns>Returns a Playabe with PrefabControlPlayable behaviour attached.</returns>
        public static ScriptPlayable<PrefabControlPlayable> Create(PlayableGraph graph, GameObject prefabGameObject, Transform parentTransform)
        {
            if (prefabGameObject == null)
                return ScriptPlayable<PrefabControlPlayable>.Null;

            var handle = ScriptPlayable<PrefabControlPlayable>.Create(graph);
            handle.GetBehaviour().Initialize(prefabGameObject, parentTransform);
            return handle;
        }

        /// <summary>
        /// The instance of the prefab created by this behaviour
        /// </summary>
        public GameObject prefabInstance
        {
            get { return m_Instance; }
        }

        /// <summary>
        /// Initializes the behaviour with a prefab and parent transform
        /// </summary>
        /// <param name="prefabGameObject">The prefab to instantiate from</param>
        /// <param name="parentTransform">Transform to parent instance to. Can be null.</param>
        /// <returns>The created instance</returns>
        public GameObject Initialize(GameObject prefabGameObject, Transform parentTransform)
        {
            if (prefabGameObject == null)
                throw new ArgumentNullException("Prefab cannot be null");

            if (m_Instance != null)
            {
                Debug.LogWarningFormat("Prefab Control Playable ({0}) has already been initialized with a Prefab ({1}).", prefabGameObject.name, m_Instance.name);
            }
            else
            {
                #if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    m_Instance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefabGameObject, parentTransform);
                    UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabUpdated;
                }
                else
                #endif
                {
                    m_Instance = Object.Instantiate(prefabGameObject, parentTransform, false);
                }
                m_Instance.name = prefabGameObject.name + " [Timeline]";
                m_Instance.SetActive(false);
                SetHideFlagsRecursive(m_Instance);
            }
            return m_Instance;
        }

        /// <summary>
        /// This function is called when the Playable that owns the PlayableBehaviour is destroyed.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        public override void OnPlayableDestroy(Playable playable)
        {
            if (m_Instance)
            {
                if (Application.isPlaying)
                    Object.Destroy(m_Instance);
                else
                    Object.DestroyImmediate(m_Instance);
            }

#if UNITY_EDITOR
            UnityEditor.PrefabUtility.prefabInstanceUpdated -= OnPrefabUpdated;
#endif
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to Playables.PlayState.Playing.
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (m_Instance == null)
                return;

            m_Instance.SetActive(true);

#if UNITY_EDITOR
            m_IsActiveCached = true;
#endif
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to PlayState.Paused.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // OnBehaviourPause can be called if the graph is stopped for a variety of reasons
            //  the effectivePlayState will test if the pause is due to the clip being out of bounds
            if (m_Instance != null && info.effectivePlayState == PlayState.Paused)
            {
                m_Instance.SetActive(false);
#if UNITY_EDITOR
                m_IsActiveCached = false;
#endif
            }
        }

#if UNITY_EDITOR
        void OnPrefabUpdated(GameObject go)
        {
            if (go == m_Instance)
            {
                SetHideFlagsRecursive(go);
                go.SetActive(m_IsActiveCached);
            }
        }

#endif

        static void SetHideFlagsRecursive(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            if (!Application.isPlaying)
                gameObject.hideFlags |= HideFlags.HideInHierarchy;
            foreach (Transform child in gameObject.transform)
            {
                SetHideFlagsRecursive(child.gameObject);
            }
        }
    }
}

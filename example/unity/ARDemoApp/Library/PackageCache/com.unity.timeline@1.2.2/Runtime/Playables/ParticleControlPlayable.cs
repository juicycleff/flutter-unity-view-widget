using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Playable that synchronizes a particle system simulation.
    /// </summary>
    public class ParticleControlPlayable : PlayableBehaviour
    {
        const float kUnsetTime = -1;
        float m_LastTime = kUnsetTime;
        uint m_RandomSeed = 1;

        // particleSystem.time can not be relied on for an accurate time. It does not advance until a delta threshold is reached(fixedUpdate) and until the start delay has elapsed.
        float m_SystemTime;

        /// <summary>
        /// Creates a Playable with a ParticleControlPlayable behaviour attached
        /// </summary>
        /// <param name="graph">The PlayableGraph to inject the Playable into.</param>
        /// <param name="component">The particle systtem to control</param>
        /// <param name="randomSeed">A random seed to use for particle simulation</param>
        /// <returns>Returns the created Playable.</returns>
        public static ScriptPlayable<ParticleControlPlayable> Create(PlayableGraph graph, ParticleSystem component, uint randomSeed)
        {
            if (component == null)
                return ScriptPlayable<ParticleControlPlayable>.Null;

            var handle = ScriptPlayable<ParticleControlPlayable>.Create(graph);
            handle.GetBehaviour().Initialize(component, randomSeed);
            return handle;
        }

        /// <summary>
        /// The particle system to control
        /// </summary>
        public ParticleSystem particleSystem { get; private set; }

        /// <summary>
        /// Initializes the behaviour with a particle system and random seed.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="randomSeed"></param>
        public void Initialize(ParticleSystem ps, uint randomSeed)
        {
            m_RandomSeed = Math.Max(1, randomSeed);
            particleSystem = ps;
            m_SystemTime = 0;
            SetRandomSeed();

            #if UNITY_EDITOR
            if (!Application.isPlaying && UnityEditor.PrefabUtility.IsPartOfPrefabInstance(ps))
                UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabUpdated;
            #endif
        }

        #if UNITY_EDITOR
        /// <summary>
        /// This function is called when the Playable that owns the PlayableBehaviour is destroyed.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        public override void OnPlayableDestroy(Playable playable)
        {
            if (!Application.isPlaying)
                UnityEditor.PrefabUtility.prefabInstanceUpdated -= OnPrefabUpdated;
        }

        void OnPrefabUpdated(GameObject go)
        {
            // When the instance is updated from, this will cause the next evaluate to resimulate.
            if (UnityEditor.PrefabUtility.GetRootGameObject(particleSystem) == go)
                m_LastTime = kUnsetTime;
        }

        #endif

        void SetRandomSeed()
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var systems = particleSystem.gameObject.GetComponentsInChildren<ParticleSystem>();
            uint seed = m_RandomSeed;
            foreach (var ps in systems)
            {
                // don't overwrite user set random seeds
                if (ps.useAutoRandomSeed)
                {
                    ps.useAutoRandomSeed = false;
                    ps.randomSeed = seed;
                    seed++;
                }
            }
        }

        /// <summary>
        /// This function is called during the PrepareFrame phase of the PlayableGraph.
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="data">A FrameData structure that contains information about the current frame context.</param>
        public override void PrepareFrame(Playable playable, FrameData data)
        {
            if (particleSystem == null || !particleSystem.gameObject.activeInHierarchy)
                return;

            float localTime = (float)playable.GetTime();
            bool shouldUpdate = Mathf.Approximately(m_LastTime, kUnsetTime) ||
                !Mathf.Approximately(m_LastTime, localTime);
            if (shouldUpdate)
            {
                float epsilon = Time.fixedDeltaTime * 0.5f;
                float simTime = localTime;
                float expectedDelta = simTime - m_LastTime;

                //  The first iteration includes the start delay. Evaluate(particleSystem.randomSeed) is how the particle system generates the random value internally.
                float startDelay = particleSystem.main.startDelay.Evaluate(particleSystem.randomSeed);
                float particleSystemDurationLoop0 = particleSystem.main.duration + startDelay;

                // The particle system time does not include the start delay so we need to remove this for our own system time.
                float expectedSystemTime = simTime > particleSystemDurationLoop0 ? m_SystemTime : m_SystemTime - startDelay;

                // conditions for restart
                bool restart = (simTime < m_LastTime) || // time went backwards
                    (simTime < epsilon) || // time is set to 0
                    Mathf.Approximately(m_LastTime, kUnsetTime) || // object disabled
                    (expectedDelta > particleSystem.main.duration) || // large jump (bug workaround)
                    !(Mathf.Abs(expectedSystemTime - particleSystem.time) < Time.maximumParticleDeltaTime); // particle system isn't where we left it
                if (restart)
                {
                    // work around for a bug where simulate(simTime, true, true) doesn't work on loops
                    particleSystem.Simulate(0, true, true);
                    particleSystem.Simulate(simTime, true, false);
                    m_SystemTime = simTime;
                }
                else
                {
                    // ps.time will wrap, so we need to account for that in computing delta time
                    float particleSystemDuration = simTime > particleSystemDurationLoop0 ? particleSystem.main.duration : particleSystemDurationLoop0;
                    float fracTime = simTime % particleSystemDuration;
                    float deltaTime = fracTime - m_SystemTime;

                    if (deltaTime < -epsilon) // detect wrapping of ps.time
                        deltaTime = fracTime + particleSystemDurationLoop0 - m_SystemTime;

                    particleSystem.Simulate(deltaTime, true, false);
                    m_SystemTime += deltaTime;
                }

                m_LastTime = localTime;
            }
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to Playables.PlayState.Playing.
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            m_LastTime = kUnsetTime;
        }

        /// <summary>
        /// This function is called when the Playable play state is changed to PlayState.Paused.
        /// </summary>
        /// <param name="playable">The playable this behaviour is attached to.</param>
        /// <param name="info">A FrameData structure that contains information about the current frame context.</param>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            m_LastTime = kUnsetTime;
        }
    }
}

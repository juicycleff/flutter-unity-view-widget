using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

namespace UnityEditor.XR.ARFoundation
{
    internal static class SceneUtils
    {
        static readonly string k_DebugFaceMaterial = "Packages/com.unity.xr.arfoundation/Materials/DebugFace.mat";

        static readonly string k_DebugPlaneMaterial = "Packages/com.unity.xr.arfoundation/Materials/DebugPlane.mat";

        static readonly string k_ParticleMaterial = "Default-Particle.mat";

        static readonly string k_LineMaterial = "Default-Line.mat";

        static readonly Color k_ParticleColor = new Color(253f / 255f, 184f / 255f, 19f / 255f);

        static readonly float k_ParticleSize = 0.02f;

        [MenuItem("GameObject/XR/AR Session Origin", false, 10)]
        static void CreateARSessionOrigin()
        {
            var originGo = ObjectFactory.CreateGameObject("AR Session Origin", typeof(ARSessionOrigin));
            var cameraGo = ObjectFactory.CreateGameObject("AR Camera",
                typeof(Camera),
                typeof(ARPoseDriver),
                typeof(ARCameraManager),
                typeof(ARCameraBackground));

            Undo.SetTransformParent(cameraGo.transform, originGo.transform, "Parent camera to session origin");

            var camera = cameraGo.GetComponent<Camera>();
            // Enforce local transform as identity for new ARSessionOrigins
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
            camera.clearFlags = CameraClearFlags.Color;
            camera.backgroundColor = Color.black;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 20f;

            var origin = originGo.GetComponent<ARSessionOrigin>();
            origin.camera = camera;
        }

        [MenuItem("GameObject/XR/AR Session", false, 10)]
        static void CreateARSession()
        {
            ObjectFactory.CreateGameObject("AR Session", typeof(ARSession), typeof(ARInputManager));
        }

        [MenuItem("GameObject/XR/AR Default Point Cloud", false, 10)]
        static void CreateARPointCloudVisualizer()
        {
            var go = ObjectFactory.CreateGameObject("AR Default Point Cloud",
                typeof(ARPointCloudParticleVisualizer));
            var particleSystem = go.GetComponent<ParticleSystem>();
            UnityEditorInternal.ComponentUtility.MoveComponentDown(particleSystem);
            UnityEditorInternal.ComponentUtility.MoveComponentDown(particleSystem);
            var main = particleSystem.main;
            main.loop = false;
            main.startSize = k_ParticleSize;
            main.startColor = k_ParticleColor;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.playOnAwake = false;

            var emission = particleSystem.emission;
            emission.enabled = false;

            var shape = particleSystem.shape;
            shape.enabled = false;

            var renderer = particleSystem.GetComponent<Renderer>();
            renderer.material = AssetDatabase.GetBuiltinExtraResource<Material>(k_ParticleMaterial);
        }

        [MenuItem("GameObject/XR/AR Default Plane", false, 10)]
        static void CreateARPlaneVisualizer()
        {
            var go = ObjectFactory.CreateGameObject("AR Default Plane",
                typeof(ARPlaneMeshVisualizer), typeof(MeshCollider), typeof(MeshFilter),
                typeof(MeshRenderer), typeof(LineRenderer));
            SetupMeshRenderer(go.GetComponent<MeshRenderer>(), k_DebugPlaneMaterial);
            SetupLineRenderer(go.GetComponent<LineRenderer>());
        }

        [MenuItem("GameObject/XR/AR Default Face", false, 10)]
        static void CreateARFaceVisualizer()
        {
            var go = ObjectFactory.CreateGameObject("AR Default Face",
                typeof(ARFaceMeshVisualizer), typeof(MeshCollider), typeof(MeshFilter),
                typeof(MeshRenderer));
            var meshRenderer = go.GetComponent<MeshRenderer>();
            SetupMeshRenderer(meshRenderer, k_DebugFaceMaterial);
            //self shadowing doesn't look good on the default face
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        static void SetupLineRenderer(LineRenderer lineRenderer)
        {
            var materials = new Material[1];
            materials[0] = AssetDatabase.GetBuiltinExtraResource<Material>(k_LineMaterial);
            lineRenderer.materials = materials;
            lineRenderer.loop = true;
            var curve = new AnimationCurve();
            curve.AddKey(0f, 0.005f);
            lineRenderer.widthCurve = curve;
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
            lineRenderer.numCornerVertices = 4;
            lineRenderer.numCapVertices = 4;
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.receiveShadows = false;
            lineRenderer.useWorldSpace = false;
        }

        static void SetupMeshRenderer(MeshRenderer meshRenderer, string materialName)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialName);
            meshRenderer.materials = new Material[] { material };
        }
    }
}

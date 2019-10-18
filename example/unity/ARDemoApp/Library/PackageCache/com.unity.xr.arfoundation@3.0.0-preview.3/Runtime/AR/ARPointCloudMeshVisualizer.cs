using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Renders an <see cref="ARPointCloud"/> as a <c>Mesh</c> with <c>MeshTopology.Points</c>.
    /// </summary>
    [RequireComponent(typeof(ARPointCloud))]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1&subfolder=/api/UnityEngine.XR.ARFoundation.ARPointCloudMeshVisualizer.html")]
    public sealed class ARPointCloudMeshVisualizer : MonoBehaviour
    {
        /// <summary>
        /// Get the <c>Mesh</c> that this visualizer creates and manages.
        /// </summary>
        public Mesh mesh { get; private set; }

        void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
        {
            s_Vertices.Clear();
            if (m_PointCloud.positions.HasValue)
            {
                foreach (var point in m_PointCloud.positions)
                    s_Vertices.Add(point);
            }

            mesh.Clear();
            mesh.SetVertices(s_Vertices);

            var indices = new int[s_Vertices.Count];
            for (int i = 0; i < s_Vertices.Count; ++i)
            {
                indices[i] = i;
            }

            mesh.SetIndices(indices, MeshTopology.Points, 0);

            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
                meshFilter.sharedMesh = mesh;
        }

        void Awake()
        {
            mesh = new Mesh();
            m_PointCloud = GetComponent<ARPointCloud>();
        }

        void OnEnable()
        {
            m_PointCloud.updated += OnPointCloudChanged;
            UpdateVisibility();
        }

        void OnDisable()
        {
            m_PointCloud.updated -= OnPointCloudChanged;
            UpdateVisibility();
        }

        void Update()
        {
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            var visible =
                enabled &&
                (m_PointCloud.trackingState != TrackingState.None);

            SetVisible(visible);
        }

        void SetVisible(bool visible)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = visible;
        }

        ARPointCloud m_PointCloud;

        static List<Vector3> s_Vertices = new List<Vector3>();
    }
}

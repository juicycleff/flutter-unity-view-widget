using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Generator functions for <see cref="ARPlane"/> mesh geometery.
    /// </summary>
    /// <remarks>
    /// These static class provides ways to generate different parts of plane geometry, such as vertices, indices, normals and UVs.
    /// You can use these functions to build an ARPlane Mesh object.
    /// </remarks>
    public static class ARPlaneMeshGenerators
    {
        /// <summary>
        /// Generates a <c>Mesh</c> from the given parameters. The <paramref name="convexPolygon"/> is assumed to be convex.
        /// </summary>
        /// <remarks>
        /// <paramref name="convexPolygon"/> is not checked for its convexness. Concave polygons will produce incorrect results.
        /// </remarks>
        /// <param name="mesh">The <c>Mesh</c> to write results to.</param>
        /// <param name="convexPolygon">The vertices of the plane's boundary, in plane-space.</param>
        /// <param name="areaTolerance">If any triangle in the generated mesh is less than this, then the entire mesh is ignored.
        /// This handles an edge case which prevents degenerate or very small triangles. Units are meters-squared.</param>
        /// <returns><c>True</c> if the <paramref name="mesh"/> was generated, <c>False</c> otherwise. The <paramref name="mesh"/> may
        /// fail to generate if it is not a valid polygon (too few vertices) or if it contains degenerate triangles (area smaller than <paramref name="areaTolerance"/>).</returns>
        public static bool GenerateMesh(Mesh mesh, Pose pose, NativeArray<Vector2> convexPolygon, float areaTolerance = 1e-6f)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");

            if (convexPolygon.Length < 3)
                return false;

            // Vertices
            s_Vertices.Clear();
            var center = Vector3.zero;
            foreach (var point2 in convexPolygon)
            {
                var point3 = new Vector3(point2.x, 0, point2.y);
                center += point3;
                s_Vertices.Add(point3);
            }
            center /= convexPolygon.Length;
            s_Vertices.Add(center);

            // If the polygon is too small or degenerate, no mesh is created.
            if (!GenerateIndices(s_Indices, s_Vertices, areaTolerance))
                return false;

            // We can't fail after this point, so it is safe to mutate the mesh
            mesh.Clear();
            mesh.SetVertices(s_Vertices);

            // Indices
            const int subMesh = 0;
            const bool calculateBounds = true;
            mesh.SetTriangles(s_Indices, subMesh, calculateBounds);

            // UVs
            GenerateUvs(s_Uvs, pose, s_Vertices);
            mesh.SetUVs(0, s_Uvs);

            // Normals
            // Reuse the same list for normals
            var normals = s_Vertices;
            for (int i = 0; i < normals.Count; ++i)
                normals[i] = Vector3.up;

            mesh.SetNormals(normals);

            return true;
        }

        /// <summary>
        /// Generates a <c>List<Vector2></c> of UVs from the given parameters.
        /// </summary>
        /// <param name="Uvs">The <c>List<Vector2></c> to write results to.</param>
        /// <param name="vertices">The vertices of the plane's boundary, in plane-space.</param>
        public static void GenerateUvs(List<Vector2> Uvs, Pose pose, List<Vector3> vertices)
        {
            // Get the twist rotation about the plane's normal, then apply
            // its inverse to the rotation to produce the "untwisted" rotation.
            // This is similar to Swing-Twist Decomposition.
            var planeRotation = pose.rotation;
            var rotationAxis = new Vector3(planeRotation.x, planeRotation.y, planeRotation.z);
            var projection = Vector3.Project(rotationAxis, planeRotation * Vector3.up);
            var normalizedTwist = (new Vector4(projection.x, projection.y, projection.z, planeRotation.w)).normalized;
            var inverseTwist = new Quaternion(normalizedTwist.x, normalizedTwist.y, normalizedTwist.z, -normalizedTwist.w);
            var untwistedRotation = inverseTwist * pose.rotation;

            // Compute the basis vectors for the plane in session space.
            var sessionSpaceRight = untwistedRotation * Vector3.right;
            var sessionSpaceForward = untwistedRotation * Vector3.forward;

            Uvs.Clear();
            foreach (var vertex in vertices)
            {
                var vertexInSessionSpace = pose.rotation * vertex + pose.position;

                // Project onto each axis
                var uv = new Vector2(
                    Vector3.Dot(vertexInSessionSpace, sessionSpaceRight),
                    Vector3.Dot(vertexInSessionSpace, sessionSpaceForward));

                Uvs.Add(uv);
            }
        }

        /// <summary>
        /// Generates a <c>List<int></c> of indices from the given parameters, forming a triangle fan.
        /// The <paramref name="convexPolygon"/> is assumed to be convex.
        /// </summary>
        /// <remarks>
        /// <paramref name="convexPolygon"/> is not checked for its convexness. Concave polygons will produce incorrect results.
        /// </remarks>
        /// <param name="Uvs">The <c>List<int></c> to write results to.</param>
        /// <param name="convexPolygon">The vertices of the plane's boundary, in plane-space.</param>
        /// <param name="areaTolerance">If any triangle in the generated mesh is less than this, then the entire mesh is ignored.
        /// <returns><c>True</c> if the indices were generated, <c>False</c> if a triangle whose area is less than <paramref name="areaTolerance"/> is found.</returns>
        public static bool GenerateIndices(List<int> indices, List<Vector3> convexPolygon, float areaTolerance = 1e-6f)
        {
            indices.Clear();

            var numBoundaryVertices = convexPolygon.Count - 1;
            var centerIndex = numBoundaryVertices;
            var areaToleranceSquared = areaTolerance * areaTolerance;

            for (int i = 0; i < numBoundaryVertices; ++i)
            {
                int j = (i + 1) % numBoundaryVertices;

                // Stop if the area of the triangle is too small
                var a = convexPolygon[i] - convexPolygon[centerIndex];
                var b = convexPolygon[j] - convexPolygon[centerIndex];

                // Area is the magnitude of the normal / 2, so the
                // area squared is the magnitude squared / 4
                var areaSquared = Vector3.Cross(a, b).sqrMagnitude * 0.25f;
                if (areaSquared < areaToleranceSquared)
                    return false;

                indices.Add(centerIndex);
                indices.Add(i);
                indices.Add(j);
            }

            return true;
        }

        // Caches to avoid reallocing Lists during calculations
        static List<int> s_Indices = new List<int>();

        static List<Vector2> s_Uvs = new List<Vector2>();

        static List<Vector3> s_Vertices = new List<Vector3>();
    }
}
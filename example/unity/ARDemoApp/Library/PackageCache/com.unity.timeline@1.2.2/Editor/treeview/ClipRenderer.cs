using UnityEngine;

namespace UnityEditor.Timeline
{
    static class ClipRenderer
    {
        static Mesh s_Quad;
        static Material s_BlendMaterial;
        static Material s_ClipMaterial;
        static readonly Vector3[] s_Vertices = new Vector3[4];
        static readonly Vector2[] s_UVs = { new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(0f, 0f) };

        static void Initialize()
        {
            if (s_Quad == null)
            {
                s_Quad = new Mesh();
                s_Quad.hideFlags |= HideFlags.DontSave;
                s_Quad.name = "TimelineQuadMesh";

                var vertices = new Vector3[4] { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0) };
                var triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
                var colors = new Color32[4] {Color.white, Color.white, Color.white, Color.white };

                s_Quad.vertices = vertices;
                s_Quad.uv = s_UVs;
                s_Quad.colors32 = colors;
                s_Quad.SetIndices(triangles, MeshTopology.Triangles, 0);
            }

            if (s_BlendMaterial == null)
            {
                var shader = (Shader)EditorGUIUtility.LoadRequired("Editors/TimelineWindow/DrawBlendShader.shader");
                s_BlendMaterial = new Material(shader);
            }

            if (s_ClipMaterial == null)
            {
                var shader = (Shader)EditorGUIUtility.LoadRequired("Editors/TimelineWindow/ClipShader.shader");
                s_ClipMaterial = new Material(shader);
            }
        }

        public static void RenderTexture(Rect r, Texture mainTex, Texture mask, Color color)
        {
            Initialize();

            s_Vertices[0] = new Vector3(r.xMin, r.yMin, 0);
            s_Vertices[1] = new Vector3(r.xMax, r.yMin, 0);
            s_Vertices[2] = new Vector3(r.xMax, r.yMax, 0);
            s_Vertices[3] = new Vector3(r.xMin, r.yMax, 0);
            s_Quad.vertices = s_Vertices;

            s_BlendMaterial.SetTexture("_MainTex", mainTex);
            s_BlendMaterial.SetTexture("_MaskTex", mask);

            // the shader adds the color, so it needs to match the rendering space.
            // the colors were authored in gamma.
            s_BlendMaterial.SetFloat("_ManualTex2SRGB", QualitySettings.activeColorSpace == ColorSpace.Linear ? 1.0f : 0.0f);
            s_BlendMaterial.SetColor("_Color", (QualitySettings.activeColorSpace == ColorSpace.Linear) ? color.gamma : color);
            s_BlendMaterial.SetPass(0);
            UnityEngine.Graphics.DrawMeshNow(s_Quad, Handles.matrix);
        }
    }
}

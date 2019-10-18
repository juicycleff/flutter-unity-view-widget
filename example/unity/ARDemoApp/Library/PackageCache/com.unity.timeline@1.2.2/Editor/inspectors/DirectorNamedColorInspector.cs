using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
    /// <summary>
    /// Internally used Inspector
    /// </summary>
    [CustomEditor(typeof(DirectorNamedColor))]
    class DirectorNamedColorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("ToTextAsset"))
            {
                DirectorStyles.Instance.ExportSkinToFile();
            }

            if (GUILayout.Button("Reload From File"))
            {
                DirectorStyles.Instance.ReloadSkin();
                UnityEditor.Selection.activeObject = DirectorStyles.Instance.customSkin;
            }
        }
    }
}

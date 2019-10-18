using UnityEditor;
using UnityEngine;

namespace Packages.Rider.Editor
{
  public class RiderScriptEditorData:ScriptableSingleton<RiderScriptEditorData>
  {
    [SerializeField] internal bool HasChanges = true; // sln/csproj files were changed
  }
}
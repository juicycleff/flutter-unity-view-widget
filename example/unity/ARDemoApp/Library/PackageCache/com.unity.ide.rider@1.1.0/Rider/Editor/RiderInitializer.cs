using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Packages.Rider.Editor
{
    internal class RiderInitializer
    {
      public void Initialize(string editorPath)
      {
        if (EditorPluginInterop.EditorPluginIsLoadedFromAssets())
        {
          Debug.LogError($"Please delete {EditorPluginInterop.GetEditorPluginAssembly().Location}. Unity 2019.2+ loads it directly from Rider installation. To disable this, open Rider's settings, search and uncheck 'Automatically install and update Rider's Unity editor plugin'.");
          return;
        }

        var dllName = "JetBrains.Rider.Unity.Editor.Plugin.Full.Repacked.dll";
        var relPath = "../../plugins/rider-unity/EditorPlugin";
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX)
          relPath = "Contents/plugins/rider-unity/EditorPlugin";
        var dllFile = new FileInfo(Path.Combine(Path.Combine(editorPath, relPath), dllName));

        if (dllFile.Exists)
        {
          // doesn't lock assembly on disk
          var bytes = File.ReadAllBytes(dllFile.FullName);
          var pdbFile = new FileInfo(Path.ChangeExtension(dllFile.FullName, ".pdb"));
          if (pdbFile.Exists)
          {
            AppDomain.CurrentDomain.Load(bytes, File.ReadAllBytes(pdbFile.FullName));  
          }
          else
          {
            AppDomain.CurrentDomain.Load(bytes);
            // AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(dllFile.FullName)); // use this for external source debug
          }
          EditorPluginInterop.InitEntryPoint();
        }
        else
        {
          Debug.Log((object) ($"Unable to find Rider EditorPlugin {dllFile.FullName} for Unity "));
        }
      }
    }
}

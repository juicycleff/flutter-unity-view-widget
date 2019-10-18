using System;
using System.Collections;
using UnityEditor;

namespace UnityEngine.TestTools
{
    public class WaitForDomainReload : IEditModeTestYieldInstruction
    {
        public WaitForDomainReload()
        {
            ExpectDomainReload = true;
        }

        public bool ExpectDomainReload { get;  }
        public bool ExpectedPlaymodeState { get; }

        public IEnumerator Perform()
        {
            EditorApplication.UnlockReloadAssemblies();

            // Detect if AssetDatabase.Refresh was called (true) or if it will be called on next tick
            bool isAsync = EditorApplication.isCompiling;

            yield return null;

            if (!isAsync)
            {
                EditorApplication.LockReloadAssemblies();
                throw new Exception("Expected domain reload, but it did not occur");
            }

            while (EditorApplication.isCompiling)
            {
                yield return null;
            }

            if (EditorUtility.scriptCompilationFailed)
            {
                EditorApplication.LockReloadAssemblies();
                throw new Exception("Script compilation failed");
            }
        }
    }
}

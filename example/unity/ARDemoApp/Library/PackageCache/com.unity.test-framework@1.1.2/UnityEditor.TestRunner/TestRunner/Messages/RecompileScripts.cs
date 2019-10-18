using System;
using System.Collections;
using UnityEditor;

namespace UnityEngine.TestTools
{
    public class RecompileScripts : IEditModeTestYieldInstruction
    {
        public RecompileScripts() : this(true)
        {
        }

        public RecompileScripts(bool expectScriptCompilation) : this(expectScriptCompilation, true)
        {
        }

        public RecompileScripts(bool expectScriptCompilation, bool expectScriptCompilationSuccess)
        {
            ExpectScriptCompilation = expectScriptCompilation;
            ExpectScriptCompilationSuccess = expectScriptCompilationSuccess;
            ExpectDomainReload = true;
        }

        public bool ExpectDomainReload { get; private set; }
        public bool ExpectedPlaymodeState { get; }
        public bool ExpectScriptCompilation { get; private set; }
        public bool ExpectScriptCompilationSuccess { get; private set; }
        public static RecompileScripts Current { get; private set; }

        public IEnumerator Perform()
        {
            Current = this;

            // We need to yield, to give the test runner a chance to prepare for the domain reload
            //  If the script compilation happens very fast, then EditModeRunner.MoveNextAndUpdateYieldObject will not have a chance to set m_CurrentYieldObject
            // This really should be fixed in EditModeRunner.MoveNextAndUpdateYieldObject
            yield return null;

            AssetDatabase.Refresh();

            if (ExpectScriptCompilation && !EditorApplication.isCompiling)
            {
                Current = null;
                throw new Exception("Editor does not need to recompile scripts");
            }

            EditorApplication.UnlockReloadAssemblies();

            while (EditorApplication.isCompiling)
            {
                yield return null;
            }

            Current = null;

            if (ExpectScriptCompilationSuccess && EditorUtility.scriptCompilationFailed)
            {
                EditorApplication.LockReloadAssemblies();
                throw new Exception("Script compilation failed");
            }
        }
    }
}

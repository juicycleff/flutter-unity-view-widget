using System;
using System.Collections;
using UnityEditor;

namespace UnityEngine.TestTools
{
    public class ExitPlayMode : IEditModeTestYieldInstruction
    {
        public bool ExpectDomainReload { get; }
        public bool ExpectedPlaymodeState { get; private set; }

        public ExitPlayMode()
        {
            ExpectDomainReload = false;
            ExpectedPlaymodeState = false;
        }

        public IEnumerator Perform()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                throw new Exception("Editor is already in EditMode");
            }

            EditorApplication.isPlaying = false;
            while (EditorApplication.isPlaying)
            {
                yield return null;
            }
        }
    }
}

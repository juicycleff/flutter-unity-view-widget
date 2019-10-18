namespace UnityEditor.TestTools.TestRunner.UnityTestProtocol
{
    internal class AssemblyCompilationErrorsMessage : Message
    {
        public string assembly;
        public string[] errors;

        public AssemblyCompilationErrorsMessage()
        {
            type = "AssemblyCompilationErrors";
        }
    }
}

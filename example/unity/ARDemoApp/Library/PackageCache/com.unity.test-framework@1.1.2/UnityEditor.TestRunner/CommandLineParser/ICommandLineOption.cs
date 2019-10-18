namespace UnityEditor.TestRunner.CommandLineParser
{
    interface ICommandLineOption
    {
        string ArgName { get; }
        void ApplyValue(string value);
    }
}

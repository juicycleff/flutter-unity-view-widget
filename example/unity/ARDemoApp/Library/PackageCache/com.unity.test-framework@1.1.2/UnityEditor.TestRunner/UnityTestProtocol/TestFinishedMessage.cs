namespace UnityEditor.TestTools.TestRunner.UnityTestProtocol
{
    internal class TestFinishedMessage : Message
    {
        public string name;
        public TestState state;
        public string message;
        public ulong duration; // milliseconds
        public ulong durationMicroseconds;

        public TestFinishedMessage()
        {
            type = "TestStatus";
            phase = "End";
        }
    }
}

namespace UnityEditor.TestTools.TestRunner.Api
{
    public interface IErrorCallbacks : ICallbacks
    {
        void OnError(string message);
    }
}

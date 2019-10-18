namespace UnityEditor.TestTools.TestRunner
{
    interface ITestSettingsDeserializer
    {
        ITestSettings GetSettingsFromJsonFile(string jsonFilePath);
    }
}

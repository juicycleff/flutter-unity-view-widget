namespace UnityEditor.TestTools
{
    public interface ITestPlayerBuildModifier
    {
        BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions);
    }
}
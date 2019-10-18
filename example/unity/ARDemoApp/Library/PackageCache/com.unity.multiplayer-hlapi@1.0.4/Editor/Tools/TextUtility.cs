using UnityEngine;

public static class TextUtility
{
    public static GUIContent TextContent(string name, string tooltip)
    {
        GUIContent newContent = new GUIContent(name);
        newContent.tooltip = tooltip;
        return newContent;
    }

    public static GUIContent TextContent(string name)
    {
        return new GUIContent(name);
    }
}
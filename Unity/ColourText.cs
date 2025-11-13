using UnityEngine;

public static class ColourText
{
    public static string Wrap(string text, Color colour)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(colour)}>{text}</color>";
    }
}
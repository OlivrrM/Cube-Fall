using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for all com.olivrm games since Jumpy Coupe

public class Utilities : MonoBehaviour
{
    public static int BoolToInt(bool boolean){
        if (boolean) return 1;
        return 0;
    }
    public static int BoolToIntn(bool boolean){
        if (boolean) return 1;
        return -1;
    }
    public static bool IntToBool(int integer)
    {
        if (integer == 1) return true;
        return false;
    }
    public static bool CoinFlip()
    {
        int dice = Random.Range(0, 2);
        return IntToBool(dice);
    }
    public static float CoinFlipn()
    {
        int dice = 0;
        while (true){
            dice = Random.Range(-1, 2);
            if (dice != 0) break;}

        return dice;
    }
    public static Color Invisible(Color targetColor)
    {
        return new Color(targetColor.r, targetColor.g, targetColor.b, 0);
    }
    public static float DifferenceBetweenn(float a,float b)
    {
        float answer = 0;
        if (a > b) answer = -(a - b);
        else answer = b - a;
        return answer;
    }
    public static float RoundUpFloat(float n, int places)
    {
        return Mathf.Ceil(n * Mathf.Pow(10, places)) / Mathf.Pow(10, places);
    }

    public static int GetArrayWrappedIndex(int arrayLength,int targetIndex,int offset)
    {
        int offsetLeft = offset;
        int currentTargetIndex = targetIndex;
        int direction = 0;
        if (offset > 0) direction = 1;
        else if (offset < 0) direction = -1;
        else return targetIndex;
        for (int i = 0; i < Mathf.Abs(offset); i++){
            currentTargetIndex += direction;
            if (currentTargetIndex < 0) currentTargetIndex = arrayLength - 1;
            else if (currentTargetIndex >= arrayLength) currentTargetIndex = 0;
            offsetLeft--;}
        return currentTargetIndex;
    }
    public static string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
    public static Vector2 DisplayPosToViewportPos(Vector2 displayPos) //Currently only designed for mobile
    {
        Vector2 displaySize = new Vector2(Display.displays[GetCurrentDisplayNumber()].systemWidth, Display.displays[GetCurrentDisplayNumber()].systemHeight);
        return displayPos/displaySize;
    }
    public static Vector2 ViewportPosToCanvasPos(Vector2 viewportPos) //Only works when canvas is set to screen space and is scaling with screen size
    {
        return viewportPos*new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        //return viewportPos * new Vector2(750, 750);
    }
    public static int GetCurrentDisplayNumber()
    {
        if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer){ //Supported platforms
            List<DisplayInfo> displayLayout = new List<DisplayInfo>();
            Screen.GetDisplayLayout(displayLayout);
            return displayLayout.IndexOf(Screen.mainWindowDisplayInfo);
        }
        else{ //Unsupported platform
            return 0; //Display.main ID
        }
    }
    public static string StringBetweenTwoStrings(string str, string FirstString, string LastString)
    {
        string FinalString;
        if (str.Contains(FirstString) && str.Contains(LastString))
        {
            int Pos1 = str.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = str.IndexOf(LastString, Pos1);
            FinalString = str.Substring(Pos1, Pos2 - Pos1);
        }
        else { throw new System.Exception("Threw an exception on 'Utilities.StringBetweenTwoStrings' cus codebase has already been designed to work with errors and not clean null returns. Lol"); }
        return FinalString;
    }
    public static Vector3 V3All(float value) { return new Vector3(value, value, value); }
    public static Vector2 V2All(float value) { return new Vector2(value, value); }
    public static Color InvertHue(Color originalColor)
    {
        float ogAlpha = originalColor.a;
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);
        h = (h + 0.5f) % 1.0f;
        Color invertedColor = Color.HSVToRGB(h, s, v);
        invertedColor.a = ogAlpha;
        return invertedColor;
    }
    public static Color DarkenColByPercent(Color originalColor, float percent)
    {
        float ogAlpha = originalColor.a;
        percent = Mathf.Clamp(percent, 0f, 100f);
        Color.RGBToHSV(originalColor, out float h, out float s, out float v);
        v = v * (1 - percent / 100f);
        Color darkenedColor = Color.HSVToRGB(h, s, v);
        darkenedColor.a = ogAlpha;
        return darkenedColor;
    }
    public static Color InvertCol(Color originalColor)
    {
        return new Color(-originalColor.r + 1, -originalColor.g + 1, -originalColor.b + 1, originalColor.a);
    }

    private static float hue = 0.0f;
    private static float saturation = 1.0f;
    private static float value = 1.0f;

    public static Color GetRainbowColor(float speed)
    {
        hue += speed * Time.deltaTime;
        if (hue >= 1.0f){hue = 0.0f;}
        return HSVToRGB(hue, saturation, value);
    }

    private static Color HSVToRGB(float hue, float saturation, float value)
    {
        int hi = Mathf.FloorToInt(hue * 6) % 6;
        float f = hue * 6 - Mathf.Floor(hue * 6);
        value *= 255.0f;
        int v = Mathf.RoundToInt(value);
        int p = Mathf.RoundToInt(value * (1 - saturation));
        int q = Mathf.RoundToInt(value * (1 - f * saturation));
        int t = Mathf.RoundToInt(value * (1 - (1 - f) * saturation));
        switch (hi)
        {
            case 0:
                return new Color32((byte)v, (byte)t, (byte)p, 255);
            case 1:
                return new Color32((byte)q, (byte)v, (byte)p, 255);
            case 2:
                return new Color32((byte)p, (byte)v, (byte)t, 255);
            case 3:
                return new Color32((byte)p, (byte)q, (byte)v, 255);
            case 4:
                return new Color32((byte)t, (byte)p, (byte)v, 255);
            case 5:
                return new Color32((byte)v, (byte)p, (byte)q, 255);
            default:
                throw new System.Exception("Invalid hue value");
        }
    }
}

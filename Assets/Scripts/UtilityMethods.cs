using System;
using UnityEngine;

static class UtilityMethods
{
    public static float RoundToNearestFactor(float value, float factor)
    {
        return (float) Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;
    }

    public static int KeyCodeToInt(KeyCode key)
    {
        // Keycode 0 actually starts at 48, subtract this value from the keycode and it will return the correct int value for alpha keycodes
        return (int)key - (int)KeyCode.Alpha0;
    }
}

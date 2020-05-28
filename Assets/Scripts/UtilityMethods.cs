using System;
using System.Collections;
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

    public static void SetTimeout(this MonoBehaviour monoBehaviour, Action action, float seconds)
    {
        monoBehaviour.StartCoroutine(TimeoutEnumerator(action, seconds));
        
    }

    private static IEnumerator TimeoutEnumerator(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
}

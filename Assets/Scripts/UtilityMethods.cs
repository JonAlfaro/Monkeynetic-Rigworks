using System;
using UnityEngine;

static class UtilityMethods
{
    public static float RoundToNearestFactor(float value, float factor)
    {
        return (float) Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class Remap
{
    public static float RemapJessy(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    public static float RemapRaza(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = value - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    public static float RemapKru(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float normal = Mathf.InverseLerp(fromMin, fromMax, value);
        float bValue = Mathf.Lerp(toMin, toMax, normal);

        return bValue;
    }

}

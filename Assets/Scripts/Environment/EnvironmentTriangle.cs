using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnvironmentTriangle
{
    [HideInInspector] public Vector3 CenterPos;
    public Color Color;
    [HideInInspector] public Vector3 ColorValues;
    public Vector3 EnvironmentValues;

    public EnvironmentTriangle(Vector3 center, Vector3 values) {
        CenterPos = center;
        EnvironmentValues = values + new Vector3(5, 5, 5);

        var maxValue = Mathf.Max(EnvironmentValues.x, EnvironmentValues.y, EnvironmentValues.z);
        ColorValues = EnvironmentValues / maxValue;
        Color = new Color(ColorValues.x, ColorValues.y, ColorValues.z);
    }

    public void SetEnvironmentValues(Vector3 values)
    {
        EnvironmentValues = values;
        EnvironmentValuesUpdated();
    }

    public void AddEnvironmentValues(Vector3 values) {
        EnvironmentValues += values;
        EnvironmentValuesUpdated();
    }

    public void EnvironmentValuesUpdated() {
        Color = GetColorOnEnvironmentValues(EnvironmentValues);
    }

    public static Color GetColorOnEnvironmentValues(Vector3 environmentValues) {
        var maxValue = Mathf.Max(environmentValues.x, environmentValues.y, environmentValues.z);
        var ColorValues = environmentValues / maxValue;
        return new Color(ColorValues.x, ColorValues.y, ColorValues.z);
    }

    public override string ToString()
    {
        return $"{EnvironmentValues} : ";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlantData
{
    public GameObject PlantPrefab;
    public Vector3 EnvironmentValues;
    public Vector3Int ExtraPointsByAlpacas;
    public float Radius;
    public float MinRadius;

    public int MaxPoints;
    public Sprite PlantSprite;

    public List<PlantTarget> PlantTargets;
}

[Serializable]
public struct PlantTarget
{
    public Vector3 TargetValues;
    public float Radius;
    public int AddValue;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantGeneratorData", menuName = "Plant/PlantGeneratorData", order = 0)]
public class PlantGeneratorData : ScriptableObject
{
    public GameObject PlantPrefab;
    [Range(10, 50)]
    public int Points = 10;
    public int Rarity = 5;
    public int StartPack = 0;

    public float MinRadius = 1f;
    public float Radius = 2f;

    public Sprite PlantSprite;

    public string OpenKey;

    //[Header("Possible colors")]
    //[MinTo(-25, 25, "R")]
    //public float RMin = 0;
    //public float R = 10;
    //[MinTo(-25, 25, "G")]
    //public float GMin = 0;
    //public float G = 10;
    //[MinTo(-25, 25, "B")]
    //public float BMin = 0;
    //public float B = 10;
}

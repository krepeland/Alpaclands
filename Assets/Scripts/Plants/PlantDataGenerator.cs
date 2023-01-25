using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDataGenerator : MonoBehaviour
{
    public static PlantDataGenerator singleton;
    public static Dictionary<int, List<PlantGeneratorData>> PlantGeneratorDatas;
    private List<PlantGeneratorData> possibleDatas;

    private int packIndex = -1;
    private int totalRarity = 0;

    private List<List<Vector3>> allColors = new List<List<Vector3>>() {
        new List<Vector3>() { new Vector3(2, 6, 2), new Vector3(2, 2, 6), new Vector3(6, 2, 2) },
        new List<Vector3>() { new Vector3(0, 15, 0), new Vector3(0, 0, 15), new Vector3(15, 0, 0), new Vector3(3.33f, 3.33f, 3.33f)},
        new List<Vector3>() { new Vector3(4.5f, 4.5f, 1), new Vector3(1, 4.5f, 4.5f), new Vector3(4.5f, 1, 4.5f) },
        new List<Vector3>() { new Vector3(6, 12, 2), new Vector3(2, 12, 6), new Vector3(12, 6, 2), new Vector3(2, 6, 12), new Vector3(12, 2, 6), new Vector3(6, 2, 12) },
    };

    private List<List<Vector3Int>> extraPointByAlpacasValues = new List<List<Vector3Int>>() {
        new List<Vector3Int>() { new Vector3Int(0, 1, 0), new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 0) },
        new List<Vector3Int>() { new Vector3Int(0, 1, 0), new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 0), new Vector3Int(1, 1, 1)},
        new List<Vector3Int>() { new Vector3Int(1, 1, 0), new Vector3Int(0, 1, 1), new Vector3Int(1, 0, 1) },
        new List<Vector3Int>() { new Vector3Int(1, 1, 0), new Vector3Int(0, 1, 1), new Vector3Int(1, 1, 0), new Vector3Int(0, 1, 1), new Vector3Int(1, 0, 1), new Vector3Int(1, 0, 1) },
    };

    //private List<List<Vector3>> allColors = new List<List<Vector3>>() {
    //    new List<Vector3>() { new Vector3(2, 6, 2), new Vector3(2, 2, 6), new Vector3(6, 2, 2) },
    //    new List<Vector3>() { new Vector3(1, 8, 1), new Vector3(1, 1, 8), new Vector3(8, 1, 1), new Vector3(3.33f, 3.33f, 3.33f)},
    //    new List<Vector3>() { new Vector3(4.5f, 4.5f, 1), new Vector3(1, 4.5f, 4.5f), new Vector3(4.5f, 1, 4.5f) },
    //    new List<Vector3>() { new Vector3(3, 6, 1), new Vector3(1, 6, 3), new Vector3(4, 4, 2), new Vector3(2, 4, 4), new Vector3(6, 3, 1), new Vector3(1, 3, 6), new Vector3(4, 2, 4), new Vector3(6, 1, 3), new Vector3(3, 1, 6) },
    //};

    private void Awake()
    {
        singleton = this;

        PlantGeneratorDatas = new Dictionary<int, List<PlantGeneratorData>>();
        possibleDatas = new List<PlantGeneratorData>();

        foreach (var e in Resources.LoadAll<PlantGeneratorData>("Plants")) {
            if (!PlantGeneratorDatas.ContainsKey(e.StartPack))
                PlantGeneratorDatas[e.StartPack] = new List<PlantGeneratorData>();
            PlantGeneratorDatas[e.StartPack].Add(e);
        }
    }

    public void AddPackIndex() {
        packIndex += 1;
        if (PlantGeneratorDatas.ContainsKey(packIndex)) {
            foreach (var e in PlantGeneratorDatas[packIndex]) {
                if (KeyManager.GetKey(e.OpenKey, 0) == 0)
                    continue;
                possibleDatas.Add(e);
                totalRarity += e.Rarity;
            }
        }
    }

    public int GetPackIndex() {
        return packIndex;
    }

    public PlantData CreatePlantData()
    {
        var randomPlant = GetRandomPlant();
        var envValues = GetRandomEnvValues(randomPlant, out var extraPointByAlpacas);

        var targetPoints = randomPlant.Points;

        var plantTargets = new List<PlantTarget>();
        plantTargets.Add(new PlantTarget() { Radius = 50, AddValue = 1, TargetValues = envValues + new Vector3(5, 5, 5) });

        var result = new PlantData() {
            MaxPoints = targetPoints,
            EnvironmentValues = envValues,
            MinRadius = randomPlant.MinRadius,
            Radius = randomPlant.Radius,
            PlantPrefab = randomPlant.PlantPrefab,
            PlantTargets = plantTargets,
            PlantSprite = randomPlant.PlantSprite,
            ExtraPointsByAlpacas = extraPointByAlpacasValues[extraPointByAlpacas.Item1][extraPointByAlpacas.Item2]
        };
        return result;
    }

    public PlantGeneratorData GetRandomPlant() {
        var targetValue = Random.Range(1, totalRarity + 1);
        for (var i = 0; i < possibleDatas.Count; i++)
        {
            targetValue -= possibleDatas[i].Rarity;
            if (targetValue <= 0)
                return possibleDatas[i];
        }
        Debug.LogError("Error random plant");
        return possibleDatas[possibleDatas.Count - 1];
    }

    public Vector3 GetRandomEnvValues(PlantGeneratorData plant, out (int, int) extraPointByAlpacas) {
        var colorPack = Random.Range(0, Mathf.Min(4, (int)((packIndex + 1) / 1.5f)));
        var colorIndex = Random.Range(0, allColors[colorPack].Count);

        extraPointByAlpacas = (colorPack, colorIndex);

        return allColors[colorPack][colorIndex];
    }
}

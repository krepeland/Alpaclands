using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GeneratorPartSettings {
    public GameObject Prefab;
    public int MaxCount;
}

public class Generator : MonoBehaviour
{
    public List<GameObject> MainPoses_Small;
    public List<GameObject> MainPoses_Medium;
    public List<GameObject> MainPoses_Big;
    public List<GeneratorPartSettings> PartObjects;
    public Dictionary<GeneratorPartSettings, int> CountPlaced;

    public List<GameObject> GeneratedParts;
    public static Generator singleton;

    private float generateT;
    private List<float> targetGenerateT;

    private void Awake()
    {
        singleton = this;
        GeneratedParts = new List<GameObject>();
        targetGenerateT = new List<float>();
        CountPlaced = new Dictionary<GeneratorPartSettings, int>();
        for(var i = 0; i < PartObjects.Count; i++) {
            var part = PartObjects[i];
            if(part.MaxCount <= 0)
            {
                PartObjects.Remove(part);
                i -= 1;
            }
        }

        var size = KeyManager.GetKey("SelectedNow", 0);
        GameObject mainPrefab = null;

        switch (size) {
            case 0:
                mainPrefab = MainPoses_Small[Random.Range(0, MainPoses_Small.Count)];
                break;
            case 1:
                mainPrefab = MainPoses_Medium[Random.Range(0, MainPoses_Medium.Count)];
                break;
            case 2:
                mainPrefab = MainPoses_Big[Random.Range(0, MainPoses_Big.Count)];
                break;
        }

        var mainPoses = Instantiate(mainPrefab, transform);
        mainPoses.transform.position = Vector3.zero;
        mainPoses.transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        var generatorPoses = mainPoses.GetComponent<GeneratorPoses>();

        foreach (var pos in generatorPoses.Poses) {
            var resultPrefab = GetRandomPartPrefab();
            var partObject = Instantiate(resultPrefab, pos);
            partObject.transform.localPosition = Vector3.zero;
            var part = partObject.GetComponent<GeneratorPart>();
            partObject.transform.localRotation =
                Quaternion.Euler(
                    Random.Range(part.MinAngles.x, part.MaxAngles.x),
                    Random.Range(part.MinAngles.y, part.MaxAngles.y),
                    Random.Range(part.MinAngles.z, part.MaxAngles.z)
                    );
            partObject.transform.localScale =
                new Vector3(
                    partObject.transform.localScale.x * Random.Range(part.MinScaleMultiply.x, part.MaxScaleMultiply.x),
                    partObject.transform.localScale.y * Random.Range(part.MinScaleMultiply.y, part.MaxScaleMultiply.y),
                    partObject.transform.localScale.z * Random.Range(part.MinScaleMultiply.z, part.MaxScaleMultiply.z)
                    );
            partObject.transform.localPosition += new Vector3(
                    Random.Range(part.MinOffset.x, part.MaxOffset.x),
                    Random.Range(part.MinOffset.y, part.MaxOffset.y),
                    Random.Range(part.MinOffset.z, part.MaxOffset.z)
                    );

            targetGenerateT.Add(Random.Range(1f, 2f));
            GeneratedParts.Add(partObject);
        }
    }

    private void Update()
    {
        generateT += Time.deltaTime;
        for (var i = 0; i < GeneratedParts.Count; i++) {
            var part = GeneratedParts[i];
            part.transform.position = new Vector3(
                part.transform.position.x,
                Mathf.Lerp(-3, 0.25f, generateT / targetGenerateT[i]),
                part.transform.position.z);
        }

        if (generateT > 2f)
        {
            foreach (var part in GeneratedParts) { 
                part.transform.position = new Vector3(
                part.transform.position.x,
                0.25f,
                part.transform.position.z);
            }
            enabled = false;
        }
    }

    public GameObject GetRandomPartPrefab() {
        var part = PartObjects[Random.Range(0, PartObjects.Count)];
        if (!CountPlaced.ContainsKey(part))
            CountPlaced[part] = 0;
        CountPlaced[part] += 1;
        if (CountPlaced[part] >= part.MaxCount)
            PartObjects.Remove(part);
        return part.Prefab;
    }
}

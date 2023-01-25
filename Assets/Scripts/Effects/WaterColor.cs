using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour
{
    [SerializeField] private Material WaterMaterial;
    [SerializeField] private List<WaterColorPreset> colorPresets;

    private static WaterColorPreset nextPreset;
    private static WaterColorPreset nowPreset;
    private static float T;

    private void Start()
    {
        nextPreset = colorPresets[Random.Range(0, colorPresets.Count)];
        ChooseNewPreset();
    }

    void Update()
    {
        T += Time.deltaTime * 0.01f;
        if (T < 1)
        {
            WaterMaterial.SetColor("Color_E0824245", Color.Lerp(nowPreset.mainColor, nextPreset.mainColor, T));
            WaterMaterial.SetColor("Color_A271762B", Color.Lerp(nowPreset.deepColor, nextPreset.deepColor, T));
            WaterMaterial.SetColor("Color_AFFD055F", Color.Lerp(nowPreset.foamColor, nextPreset.foamColor, T));
        }
        if (T >= 1.1f) {
            T = 0;
            ChooseNewPreset();
        }
    }

    void ChooseNewPreset() {
        nowPreset = nextPreset;
        nextPreset = colorPresets[Random.Range(0, colorPresets.Count)];
    }
}
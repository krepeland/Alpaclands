using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager singleton;

    [SerializeField] private Slider SoundSlider;
    [SerializeField] private Slider MusicSlider;

    public bool IsSettingsOpened;

    private static bool IsSettedAtStart;

    private List<Vector2Int> resolutions = new List<Vector2Int>() { 
        new Vector2Int(800, 600),
        new Vector2Int(1024, 768),
        new Vector2Int(1152, 864),
        new Vector2Int(1200, 600),
        new Vector2Int(1280, 720),
        new Vector2Int(1280, 768),
        new Vector2Int(1280, 1024),
        new Vector2Int(1400, 1050),
        new Vector2Int(1440, 900),
        new Vector2Int(1536, 960),
        new Vector2Int(1536, 1024),
        new Vector2Int(1600, 900),
        new Vector2Int(1600, 1024),
        new Vector2Int(1600, 1200),
        new Vector2Int(1680, 1050),
        new Vector2Int(1920, 1080)
    };

    [SerializeField] private SettingsChoose resolutionChoose;

    private void Awake()
    {
        singleton = this;
        SetOpenStateTo(false);

        SoundSlider.value = KeyManager.GetKey("Sound", 5);
        MusicSlider.value = KeyManager.GetKey("Music", 5);

        var width = Screen.currentResolution.width;
        var height = Screen.currentResolution.height;

        if (resolutions.Contains(new Vector2Int(width, height)))
        {

            if (!IsSettedAtStart)
            {
                if (KeyManager.GetKey("Resolution", -1) == -1)
                {
                    KeyManager.SetKey("Resolution", resolutions.IndexOf(new Vector2Int(width, height)));
                }
            }
        }
        else
        {
            resolutionChoose.AddValue($"{width}x{height}");
            resolutions.Add(new Vector2Int(width, height));
            if (!IsSettedAtStart)
            {
                if (KeyManager.GetKey("Resolution", -1) == -1){
                    KeyManager.SetKey("Resolution", resolutions.Count - 1);
                }
            }
        }
        UpdateResolution();

        IsSettedAtStart = true;
    }

    public void ChangeIsOpened() {
        IsSettingsOpened = !IsSettingsOpened;
        gameObject.SetActive(IsSettingsOpened);

        if (!IsSettingsOpened) {
            UpdateResolution();
        }

        SoundManager.TryPlayClickSound();
    }

    void UpdateResolution()
    {
        var resulutionID = KeyManager.GetKey("Resolution", 0);
        var isFullScreen = KeyManager.GetKey("Fullscreen", 1) == 1;
        Screen.SetResolution(resolutions[resulutionID].x, resolutions[resulutionID].y, isFullScreen);

        SoundManager.TryPlayClickSound();
    }

    public void UpdateIsFullScreen()
    {
        var isFullScreen = KeyManager.GetKey("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullScreen;

        SoundManager.TryPlayClickSound();
    }

    public void SoundsVolumeChanged() {
        KeyManager.SetKey("Sound", Mathf.RoundToInt(SoundSlider.value));
        SoundManager.TryUpdateVolume();

        SoundManager.TryPlayClickSound();
    }

    public void MusicVolumeChanged()
    {
        KeyManager.SetKey("Music", Mathf.RoundToInt(MusicSlider.value));
        MusicManager.TryUpdateVolume();

        SoundManager.TryPlayClickSound();
    }

    void SetOpenStateTo(bool state) {
        if (IsSettingsOpened != state)
        {
            ChangeIsOpened();
        }
        else
        {
            ChangeIsOpened();
            ChangeIsOpened();
        }
    }

    void Update()
    {
        
    }
}

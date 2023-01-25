using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsFullScreenButton : CustomButton
{
    Vector2 startSize;
    [SerializeField] GameObject SelectedImage;

    private void Awake()
    {
        startSize = transform.localScale;

        var isFullScreen = KeyManager.GetKey("Fullscreen", 1) == 1;
        SelectedImage.SetActive(isFullScreen);
    }

    public override void OnHover()
    {
        transform.localScale = startSize * 1.1f;
    }

    public override void OutHover()
    {
        transform.localScale = startSize;
    }

    public override void OnClicked(int mouseButton)
    {
        var isFullScreen = KeyManager.GetKey("Fullscreen", 1) == 1;
        SelectedImage.SetActive(!isFullScreen);
        KeyManager.SetKey("Fullscreen", isFullScreen ? 0 : 1);

        SoundManager.TryPlayClickSound();
        //SettingsManager.singleton.UpdateIsFullScreen();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : CustomButton
{
    Vector2 startSize;

    private void Start()
    {
        startSize = transform.localScale;
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
        SettingsManager.singleton.ChangeIsOpened();
    }
}

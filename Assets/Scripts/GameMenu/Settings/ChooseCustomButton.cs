using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCustomButton : CustomButton
{
    [SerializeField] int delta;
    [SerializeField] SettingsChoose settingsChoose;

    public override void OnClicked(int mouseButton)
    {
        settingsChoose.NextValue(delta);
    }
}

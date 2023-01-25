using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : CustomButton
{
    [SerializeField] private GameTutorial tutorial;

    public override void OnClicked(int mouseButton)
    {
        //253740
        tutorial.Skip();
    }
}

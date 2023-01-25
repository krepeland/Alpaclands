using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : CustomButton
{
    public override void OnClicked(int mouseButton)
    {
        base.OnClicked(mouseButton);
        Debug.Log(gameObject.name + " " + "clicked");
    }

    public override void OnHover()
    {
        base.OnHover();
        Debug.Log(gameObject.name + " " + "hovered");
    }

    public override void OutHover()
    {
        base.OutHover();
        Debug.Log(gameObject.name + " " + "out hovered");
    }

    public override void OnHold(int mouseButton)
    {
        base.OnHold(mouseButton);

        Debug.Log(gameObject.name + " " + "hold");
    }

    public override void OnRelease(int mouseButton)
    {
        base.OnRelease(mouseButton);

        Debug.Log(gameObject.name + " " + "release");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyStuffAccept : CustomButton
{
    public bool IsAccept;
    [SerializeField] LegacyStuff LegacyStuff;

    bool isDestroyed;

    public override void OnClicked(int mouseButton)
    {
        base.OnClicked(mouseButton);
        LegacyStuff.SetIsAccepted(IsAccept);
        isDestroyed = true;
    }

    public override void OnHover()
    {
        if (isDestroyed)
            return;
        transform.localScale = Vector3.one * 1.1f;
    }

    public override void OutHover()
    {
        if (isDestroyed)
            return;
        transform.localScale = Vector3.one;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomButton : MonoBehaviour
{
    public bool IsHovered;
    public bool IsHold;
    public virtual float GetPressedTime() { 
        return 0.2f; 
    }

    public void CallOnHover()
    {
        if (IsHovered)
            return;
        IsHovered = true;
        OnHover();
    }

    public virtual void OnHover() { 
    
    }

    public void CallOutHover()
    {
        IsHovered = false;
        IsHold = false;
        OutHover();
    }

    public virtual void OutHover() {

    }

    public void CallOnClicked(int mouseButton)
    {
        IsHold = false;
        OnClicked(mouseButton);
    }

    public virtual void OnClicked(int mouseButton) {

    }

    public void CallOnHold(int mouseButton)
    {
        IsHold = true;
        OnHold(mouseButton);
    }

    public virtual void OnHold(int mouseButton) { 
    
    }

    public void CallOnRelease(int mouseButton)
    {
        if (!IsHold)
            return;
        IsHold = false;
        OnRelease(mouseButton);
    }

    public virtual void OnRelease(int mouseButton) { 
    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHideElement2 : MonoBehaviour
{
    public static bool Hide;
    public Vector2 Direction = new Vector2(0, -1f);
    public static float Speed = 350;

    void Update()
    {
        if (!Hide)
            return;

        GetComponent<RectTransform>().anchoredPosition += Direction * Speed * Time.deltaTime;
    }
}
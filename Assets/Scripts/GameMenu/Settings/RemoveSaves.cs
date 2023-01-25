using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveSaves : CustomButton
{
    [SerializeField] float T;
    [SerializeField] Image Filler;

    public override void OnHold(int mouseButton)
    {
        base.OnHold(mouseButton);
    }

    private void Update()
    {
        if (IsHold)
        {
            T += Time.deltaTime;

            if (T >= 4) {
                DeleteSaves();
            }

            Filler.fillAmount = T / 4f;
        }
        else {
            T = 0;
            Filler.fillAmount = 0;
        }
    }

    private void DeleteSaves() {
        var keys = new List<string>() { "Sound", "Music", "Language", "Resolution", "Fullscreen" };
        var e = new List<(string, int)>();
        foreach (var key in keys) {
            if (PlayerPrefs.HasKey(key)) {
                e.Add((key, PlayerPrefs.GetInt(key)));
            }
        }
        PlayerPrefs.DeleteAll();
        foreach (var key in e)
        {
            PlayerPrefs.SetInt(key.Item1, key.Item2);
        }

        Application.Quit();
    }
}

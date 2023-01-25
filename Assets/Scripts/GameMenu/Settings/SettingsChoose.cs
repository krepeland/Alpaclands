using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsChoose : MonoBehaviour
{
    [SerializeField] string KeyName;
    [SerializeField] List<string> allValues;

    [SerializeField] Text ValueText;

    [SerializeField] bool isToUpper;
    int valueNow;
    private void Start()
    {
        SetValue(KeyManager.GetKey(KeyName, 0));
    }

    public void NextValue (int delta)
    {
        SetValue(((valueNow + delta) + allValues.Count) % allValues.Count);

        SoundManager.TryPlayClickSound();
    }

    public void AddValue(string value) {
        allValues.Add(value);
    }

    void SetValue(int value) {
        valueNow = value;
        if (isToUpper)
        {
            ValueText.text = allValues[valueNow].ToUpper();
        }
        else
        {
            ValueText.text = allValues[valueNow];
        }
        KeyManager.SetKey(KeyName, valueNow);
    }
}

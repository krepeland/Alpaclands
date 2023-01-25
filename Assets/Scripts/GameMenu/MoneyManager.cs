using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private List<Text> MoneyCountTexts;

    void Awake()
    {
        KeyManager.AddEvent("Coins", (x) => { 
            foreach(var countText in MoneyCountTexts)
                countText.text = x.ToString();
        });
    }

    private void Start()
    {
        KeyManager.SetKey("Coins", KeyManager.GetKey("Coins"));
    }
}

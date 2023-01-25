using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedLanguage { 
    English,
    German,
    French,
    Spanish,
    Italian,
    Russian,
    Chinese
}

[System.Serializable]
public struct LanguagedText {
    public SelectedLanguage Language;
    public string Text;
}

public class TextTranslator : MonoBehaviour
{
    [SerializeField] private List<LanguagedText> translations;
    private Dictionary<SelectedLanguage, string> translated;
    [SerializeField] private Text text;

    private void Awake()
    {
        translated = new Dictionary<SelectedLanguage, string>();
        foreach (var e in translations) {
            if (translated.ContainsKey(e.Language))
                continue;
            translated[e.Language] = e.Text;
        }
        KeyManager.AddEvent("Language", (x) => { UpdateText((SelectedLanguage)KeyManager.GetKey("Language", 0)); });
    }

    private void Start()
    {
        UpdateText((SelectedLanguage)KeyManager.GetKey("Language", 0));
    }

    void UpdateText(SelectedLanguage language) {
        if (!translated.ContainsKey(language))
            language = SelectedLanguage.English;

        text.text = translated[language].ToUpper();
    }
}

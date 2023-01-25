using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyManager : MonoBehaviour
{
    static Dictionary<string, int> mainData;
    static Dictionary<string, UnityAction<int>> updateActions;

    static bool IsLoaded;

    static List<string> KeysToLoad = new List<string> { 
        "Coins", "Level", "SelectedNow", "MaxScore",
        "Slot_Plant-1",
        "Slot_Plant-2-1", "Slot_Plant-2-2",
        "Slot_Plant-3-1", "Slot_Plant-3-2",
        "Slot_Plant-4-1", "Slot_Plant-4-2",
        "Slot_Plant-5-1", "Slot_Plant-5-2",
        "Slot_Plant-6",
        "Slot_Alpacas-1",
        "Slot_Alpacas-Max-1", "Slot_Alpacas-Max-2", "Slot_Alpacas-Max-3", "Slot_Alpacas-Max-4",
        "Slot_Alpacas-Rarity-1", "Slot_Alpacas-Rarity-2", "Slot_Alpacas-Rarity-3", "Slot_Alpacas-Rarity-4",
        "Slot_Alpacas-Luck-1", "Slot_Alpacas-Luck-2", "Slot_Alpacas-Luck-3", "Slot_Alpacas-Luck-4",
        "Tutorial-menu", "Tutorial-game"
    };

    static List<string> KeysToLoadIfHas = new List<string>() {
        "Sound", "Music", "Language", "Resolution", "Fullscreen", "OldLevel", "PlantCount"
    };

    private void Awake()
    {

        if (mainData == null)
            mainData = new Dictionary<string, int>();

        updateActions = new Dictionary<string, UnityAction<int>>();

        if (!IsLoaded)
        {
            LoadDatas();
            SetKey("Slot_Plant-1", 1);
            SetKey("Slot_Alpacas-1", 1);

            IsLoaded = true;
        }
    }

    private void LoadDatas() {
        foreach (var key in KeysToLoad) {
            SetKey(key, PlayerPrefs.GetInt(key, 0));
        }

        foreach (var key in KeysToLoadIfHas)
        {
            if(PlayerPrefs.HasKey(key))
                SetKey(key, PlayerPrefs.GetInt(key, 0));
        }
    }

    public static void SetKey(string key, int count) {
        mainData[key] = count;

        if (updateActions.ContainsKey(key)) {
            updateActions[key]?.Invoke(count);
        }

        PlayerPrefs.SetInt(key, mainData[key]);
    }

    public static void AddToKey(string key, int count)
    {
        if (!mainData.ContainsKey(key))
            mainData[key] = 0;
        
        mainData[key] += count;

        if (updateActions.ContainsKey(key))
        {
            updateActions[key]?.Invoke(mainData[key]);
        }

        PlayerPrefs.SetInt(key, mainData[key]);
    }

    public static void AddEvent(string key, UnityAction<int> unityEvent) {
        if (!updateActions.ContainsKey(key))
            updateActions[key] = new UnityAction<int>((x) => { });

        updateActions[key] += unityEvent;
    }

    public static int GetKey(string key, int defaultValue = 0) {
        if (!mainData.ContainsKey(key))
            return defaultValue;
        return mainData[key];
    }

    private void OnDestroy()
    {
        updateActions = new Dictionary<string, UnityAction<int>>();
    }
}

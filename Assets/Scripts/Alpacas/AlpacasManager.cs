using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct AplacaCountDisplay
{
    public AlpacaColor aplacaColor;
    public Text counter1;
    public Text counter2;
    public GameObject preview;
    public GameObject previewMesh;
}

public class AlpacasManager : MonoBehaviour
{
    public static AlpacasManager singleton;
    public LayerMask GroundMask;

    public Vector3Int ColoredAlpacasCount;

    [SerializeField] private List<AplacaCountDisplay> alpacaCountDisplays;
    [SerializeField] private GameObject AlpacaPrefab;

    private bool isRotating;
    private List<bool> rotatingStates = new List<bool>() { false, false, false };

    private Dictionary<AlpacaColor, List<Alpaca>> AlpacasByColors;

    public int AlpacasCountNow;
    public static int PacksToAlpacs = 5;
    public static int MaxAlpacasCount = 3;
    public static int Luck = 1;

    private void Awake()
    {
        singleton = this;

        MaxAlpacasCount = 3;
        if (KeyManager.GetKey("Slot_Alpacas-Max-1") != 0)
            MaxAlpacasCount += 3;
        if (KeyManager.GetKey("Slot_Alpacas-Max-2") != 0)
            MaxAlpacasCount += 3;
        if (KeyManager.GetKey("Slot_Alpacas-Max-3") != 0)
            MaxAlpacasCount += 3;
        if (KeyManager.GetKey("Slot_Alpacas-Max-4") != 0)
            MaxAlpacasCount += 3;

        PacksToAlpacs = 5;
        if (KeyManager.GetKey("Slot_Alpacas-Rarity-1") != 0)
            PacksToAlpacs -= 1;
        if (KeyManager.GetKey("Slot_Alpacas-Rarity-2") != 0)
            PacksToAlpacs -= 1;
        if (KeyManager.GetKey("Slot_Alpacas-Rarity-3") != 0)
            PacksToAlpacs -= 1;
        if (KeyManager.GetKey("Slot_Alpacas-Rarity-4") != 0)
            PacksToAlpacs -= 1;

        Luck = 5;
        if (KeyManager.GetKey("Slot_Alpacas-Luck-1") != 0)
            Luck += 2;
        if (KeyManager.GetKey("Slot_Alpacas-Luck-2") != 0)
            Luck += 1;
        if (KeyManager.GetKey("Slot_Alpacas-Luck-3") != 0)
            Luck += 1;
        if (KeyManager.GetKey("Slot_Alpacas-Luck-4") != 0)
            Luck += 1;

        AlpacasByColors = new Dictionary<AlpacaColor, List<Alpaca>>();
        AlpacasByColors[AlpacaColor.Red] = new List<Alpaca>();
        AlpacasByColors[AlpacaColor.Green] = new List<Alpaca>();
        AlpacasByColors[AlpacaColor.Blue] = new List<Alpaca>();
        UpdateAlpacaColorText(AlpacaColor.Red);
        UpdateAlpacaColorText(AlpacaColor.Green);
        UpdateAlpacaColorText(AlpacaColor.Blue);

        TakeableCoin.ReloadCoinsSet();
    }

    private void Update()
    {
        if (!isRotating)
            return;
        RotateMeshes();
    }

    public void SetRotatingState(Vector3Int states) {
        rotatingStates[0] = states.x != 0;
        rotatingStates[1] = states.y != 0;
        rotatingStates[2] = states.z != 0;
        isRotating = rotatingStates[0] || rotatingStates[1] || rotatingStates[2];
        RotateMeshes();
    }

    private void RotateMeshes() {
        for (var i = 0; i < alpacaCountDisplays.Count; i++)
        {
            if (rotatingStates[i])
                alpacaCountDisplays[i].previewMesh.GetComponent<RectTransform>().Rotate(Vector3.forward, Time.deltaTime * 90, Space.Self);
            else
                alpacaCountDisplays[i].previewMesh.GetComponent<RectTransform>().localRotation = Quaternion.Euler(-90, 90, 0);
        }
    }

    public bool CheckIsPositionSuitable(Vector3 position, out RaycastHit hit)
    {
        var ray = new Ray(new Vector3(position.x, 25, position.z), new Vector3(0, -50, 0));
        if (Physics.Raycast(ray, out hit, 50, GroundMask)) {
            if (hit.point.y <= 0.3f)
                return false;
            return true;
        }
        return false;
    }

    public int GetExtraPoints(Vector3Int extraPointsByAlpacas) {
        return ColoredAlpacasCount.x * extraPointsByAlpacas.x
            + ColoredAlpacasCount.y * extraPointsByAlpacas.y
            + ColoredAlpacasCount.z * extraPointsByAlpacas.z;
    }

    public void AddAlpacaCount(AlpacaColor alpacaColor, Alpaca alpaca) {
        AlpacasCountNow += 1;
        switch (alpacaColor) {
            case AlpacaColor.Red:
                ColoredAlpacasCount.x += 1;
                UpdateAlpacaColorText(AlpacaColor.Red);
                break;
            case AlpacaColor.Green:
                ColoredAlpacasCount.y += 1;
                UpdateAlpacaColorText(AlpacaColor.Green);
                break;
            case AlpacaColor.Blue:
                ColoredAlpacasCount.z += 1;
                UpdateAlpacaColorText(AlpacaColor.Blue);
                break;
        }
        AlpacasByColors[alpacaColor].Add(alpaca);

        CardManager.singleton.UpdateValuesOnSpawnedCards();
    }

    public void CallAlpacasHappy(Vector3 alpacaColors)
    {
        if (alpacaColors.x != 0)
            CallAlpacasHappy(AlpacaColor.Red);
        if (alpacaColors.y != 0)
            CallAlpacasHappy(AlpacaColor.Green);
        if (alpacaColors.z != 0)
            CallAlpacasHappy(AlpacaColor.Blue);
    }

    public void CallAlpacasHappy(AlpacaColor alpacaColor) {
        foreach (var e in AlpacasByColors[alpacaColor]) {
            e.CallHappy();
        }
    }

    public void SpawnAlpaca(AlpacaColor alpacaColor) {
        var pos = Generator.singleton.GeneratedParts[Random.Range(0, Generator.singleton.GeneratedParts.Count)].transform.position;
        CheckIsPositionSuitable(pos, out var hit);
        var alpaca = Instantiate(AlpacaPrefab, hit.point, Quaternion.Euler(0, 0, 0)).GetComponent<Alpaca>();
        alpaca.SetAlpacaColor(alpacaColor);
    }

    void UpdateAlpacaColorText(AlpacaColor alpacaColor) {
        var count = 0;
        switch (alpacaColor) {
            case AlpacaColor.Red:
                count = ColoredAlpacasCount.x;
                break;
            case AlpacaColor.Green:
                count = ColoredAlpacasCount.y;
                break;
            case AlpacaColor.Blue:
                count = ColoredAlpacasCount.z;
                break;
        }
        var display = alpacaCountDisplays[(int)alpacaColor];
        var isEnabled = count > 0;
        display.preview.SetActive(isEnabled);

        display.counter1.text = count.ToString();
        display.counter2.text = count.ToString();

        UpdateDisplaysPositions();
    }

    void UpdateDisplaysPositions() {
        var posRed = -160;
        var posGreen = -80;
        if (ColoredAlpacasCount.z <= 0) {
            posRed += 80;
            posGreen += 80;
        }

        if (ColoredAlpacasCount.y <= 0)
        {
            posRed += 80;
        }

        alpacaCountDisplays[0].preview.GetComponent<RectTransform>().anchoredPosition = new Vector2(posRed, 0);
        alpacaCountDisplays[1].preview.GetComponent<RectTransform>().anchoredPosition = new Vector2(posGreen, 0);
    }
}

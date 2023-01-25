using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantPlacer : MonoBehaviour
{
    private Aim aim;
    public static PlantPlacer singleton;

    public Card TakedCard;
    public float ValueMultiply;

    public int Points;

    [SerializeField] private Text PointText;
    [SerializeField] private Text PointText2;

    private bool isTextsVisible = true;

    public int PlantPlaced;
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        aim = Aim.singleton;
        AimUpdated();
    }

    public void SetTakedCard(Card card) {
        TakedCard = card;
        if (TakedCard == null)
        {
            AlpacasManager.singleton.SetRotatingState(Vector3Int.zero);
        }
        else
        {
            AlpacasManager.singleton.SetRotatingState(card.plantData.ExtraPointsByAlpacas);
        }
        AimUpdated();
    }

    public bool TrySetPlant(Card card)
    {
        AimUpdated();

        if (!aim.CheckIsVisible())
            return false;

        var plant = Instantiate(card.plantData.PlantPrefab);
        plant.transform.position = Aim.singleton.Spawner.position;
        plant.transform.rotation = Aim.singleton.Spawner.rotation;
        plant.GetComponent<Plant>().Initialize(card.plantData);

        AlpacasManager.singleton.CallAlpacasHappy(card.plantData.ExtraPointsByAlpacas);

        ProgressBar.singleton.AddPoints(Points, plant.transform);

        KeyManager.AddToKey("PlantCount", 1);
        PlantPlaced += 1;

        AchievmentsManager.Achievment_CheckPlantCount();
        AchievmentsManager.Achievment_CheckScorePerPlant(Points);

        return true;
    }

    public void AimUpdated() {
        AimScaner.singleton.Scan();
        if (TakedCard == null)
        {
            SetIsVisiblePointText(false);
            ProgressBar.singleton.SetPointsGet(0);
            return;
        }
        if (!aim.CheckIsVisible())
        {
            SetIsVisiblePointText(false);
            return;
        }

        var resultValue = 0f;
        foreach (var target in TakedCard.plantData.PlantTargets) {
            var dist = Vector2.Distance(AimScaner.singleton.UITriangle.AimOnTrianglePos, UITriangle.GetCoordsOfEnvValue(target.TargetValues));
            var power = Mathf.Lerp(0, 1, (target.Radius - dist) / target.Radius);
            resultValue += Mathf.Sign(target.AddValue) * power;
        }

        ValueMultiply = Mathf.Clamp01(resultValue);
        Points = TakedCard.GetPointsWithValue(ValueMultiply);
        SetIsVisiblePointText(true);

        SetPointText(Points);

        ProgressBar.singleton.SetPointsGet(Points);
    }

    void SetPointText(int count) {
        PointText.text = $"+{count}";
        PointText2.text = $"+{count}";
    }

    void SetIsVisiblePointText(bool isVisible) {
        if (isVisible == isTextsVisible)
            return;
        isTextsVisible = isVisible;
        PointText.enabled = isVisible;
        PointText2.enabled = isVisible;
    }
}

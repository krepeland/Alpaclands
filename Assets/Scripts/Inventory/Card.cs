using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : CustomButton
{
    public bool IsInPack;
    public RectTransform rectTransform;
    [SerializeField] private bool isMovingToTargetPos;
    [SerializeField] private Vector2 anchoredTargetPos;
    [SerializeField] private float moveToTargetSpeed = 100;

    public PlantData plantData;

    [SerializeField] private Image CardColor;
    [SerializeField] private Image PlantImage;
    [SerializeField] private Text PointsText;

    private void Start()
    {
        var color = EnvironmentTriangle.GetColorOnEnvironmentValues(plantData.EnvironmentValues + new Vector3(5, 5, 5));
        CardColor.color = color;
        PlantImage.color = color;
        
        PointsText.text = (plantData.MaxPoints + AlpacasManager.singleton.GetExtraPoints(plantData.ExtraPointsByAlpacas)).ToString();
        PlantImage.sprite = plantData.PlantSprite;
    }

    public int GetPointsWithValue(float value) {
        return Mathf.RoundToInt(plantData.MaxPoints * value) + AlpacasManager.singleton.GetExtraPoints(plantData.ExtraPointsByAlpacas);
    }

    void Update()
    {
        if (IsInPack)
            return;
        if (isMovingToTargetPos) {
            var dist = Vector2.Distance(rectTransform.anchoredPosition, anchoredTargetPos);
            var moveValue = Time.deltaTime / dist * moveToTargetSpeed;
            if (moveValue < 1)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, anchoredTargetPos, moveValue);
            }
            else {
                isMovingToTargetPos = false;
                rectTransform.anchoredPosition = anchoredTargetPos;
            }
        }
    }

    public void UpdateValueOnCard()
    {
        PointsText.text = (plantData.MaxPoints + AlpacasManager.singleton.GetExtraPoints(plantData.ExtraPointsByAlpacas)).ToString();
    }

    public void SetTargetPos(Vector2 anchoredTargetPos)
    {
        isMovingToTargetPos = true;
        this.anchoredTargetPos = anchoredTargetPos;
    }

    public void SetTargetPos(Vector2 anchoredTargetPos, float speed)
    {
        moveToTargetSpeed = speed;
        SetTargetPos(anchoredTargetPos);
    }

    public override void OnHover()
    {
        if (IsInPack)
            return;
        if (CardManager.singleton.TakedCard != null) {
            CallOutHover();
            return;
        }
        SetCardSize(1.1f);
    }

    public override void OutHover()
    {
        if (IsInPack)
            return;
        SetCardSize(1);
    }

    public override void OnClicked(int mouseButton)
    {
        if (IsInPack)
            return;
        if (CardManager.singleton.TryTakeCard(this))
        {
            SetCardSize(1);
        }
    }

    void SetCardSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    private void OnDestroy()
    {
        CardManager.singleton.RemoveCardFromSpawnedCards(this);
    }
}

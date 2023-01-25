using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();

    public static float MaxCardOffset = 150;
    float CardOffset = 150;

    public static float ResizeToTargetSpeed = 1000;

    public float EmptySpaceOffset = 1f;

    public Card TakedCard;
    private int takedCardPosition = -1;
    private bool isInserting;
    [SerializeField] RectTransform inventoryTopBorder;
    public int positionNow;
    RectTransform CursorContainerCenter;
    RectTransform CursorContainer;

    [SerializeField] private RectTransform LeftBorder;
    [SerializeField] private RectTransform RightBorder;
    [SerializeField] private RectTransform LeftBorderCenter;
    [SerializeField] private RectTransform RightBorderCenter;

    private bool isResizingX;
    private RectTransform rectTransform;
    private float targetSizeX;

    [SerializeField] private PlantPlacer plantPlacer;

    [SerializeField] private Transform CardSpawner;
    [SerializeField] private GameObject CardPrefab;
    private List<Card> spawnedCards;

    public static CardManager singleton;

    private int spawnedCardsCount;
    private int placedPlants = 0;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        CursorContainer = UIManager.singleton.CursorContainer;
        CursorContainerCenter = UIManager.singleton.CursorContainerCenter;
        InputManager.singleton.UseStarted = ((() => UseStarted()) + InputManager.singleton.UseStarted);
        InputManager.singleton.UseCanceled = ((() => UseCanceled()) + InputManager.singleton.UseCanceled);
        InputManager.singleton.AlternativeUseCanceled += () => AlternativeUseCanceled();

        LeftBorderCenter.position = LeftBorder.position;
        RightBorderCenter.position = RightBorder.position;
        rectTransform = GetComponent<RectTransform>();
        CheckCardPosition(true);
        Aim.singleton.SwitchIsVisible(false);

        spawnedCards = new List<Card>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
    }

    public void StartCards(float t = 1.5f)
    {
        StartCoroutine(GiveCardsInTime(t + 0.25f));
    }

    IEnumerator GiveCardsInTime(float T) {
        yield return new WaitForSeconds(T);
        //GiveCardsPack();
        CardPack.singleton.AddToCount();
    }

    private void Update()
    {
        if (isResizingX)
        {
            var value = Time.deltaTime / Mathf.Abs(rectTransform.sizeDelta.x - targetSizeX) * 1500;
            if (value >= 1)
            {
                value = 1;
                isResizingX = false;
            }
            var sizeNow = Mathf.Lerp(rectTransform.sizeDelta.x, targetSizeX, value);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeNow);
        }

        if (TakedCard != null)
        {
            CheckCardPosition(false);
        }
    }

    void CheckCardPosition(bool isForceCheck) {
        var isInsertingNew = CursorContainer.anchoredPosition.y < inventoryTopBorder.anchoredPosition.y;
        var newPos = Mathf.RoundToInt(((CursorContainerCenter.anchoredPosition.x)) / CardOffset + ((Cards.Count + EmptySpaceOffset) / 2f));
        if (isForceCheck || positionNow != newPos || isInsertingNew != isInserting)
        {
            if (newPos == Cards.Count + 1)
            {
                newPos = Cards.Count;
            }
            isInserting = isInsertingNew;
            positionNow = newPos;
            RecalculateCardPositions();
        }
    }

    void RecalculateCardPositions()
    {
        for (var j = 0; j < Cards.Count; j++)
        {
            if (Cards[j] == null)
            {
                Cards.RemoveAt(j);
                j -= 1;
            }
        }

        var isHaveEmptySpace = isInserting && TakedCard != null && positionNow >= 0 && positionNow <= Cards.Count;
        int targetCount = Cards.Count + (isHaveEmptySpace ? 1 : 0);

        LeftBorderCenter.position = LeftBorder.position;
        RightBorderCenter.position = RightBorder.position;
        CardOffset = Mathf.Min(150, Mathf.Abs(LeftBorderCenter.anchoredPosition.x - RightBorderCenter.anchoredPosition.x) / targetCount);
        var cardOffset = CardOffset;

        targetSizeX = targetCount * cardOffset + 20;
        if (targetCount <= 0)
            targetSizeX = 0;
        isResizingX = true;

        float startPos = -(Cards.Count - 1 + (isHaveEmptySpace ? EmptySpaceOffset : 0)) * 0.5f * cardOffset;
        float posNow = startPos;
        int cardNow = 0;

        for (var i = 0; i < targetCount; i++) {
            if (isHaveEmptySpace && i == positionNow)
            {
                posNow += cardOffset * EmptySpaceOffset;
                continue;
            }

            var card = Cards[cardNow];
            if (card == null) {
                break;
            }

            card.rectTransform.SetSiblingIndex(cardNow);
            card.SetTargetPos(new Vector2(posNow, 0));
            posNow += cardOffset;
            cardNow++;
        }
    }

    public bool TryTakeCard(Card card) {
        if (TakedCard != null)
            return false;
        if (Cards.Contains(card))
        {
            takedCardPosition = Cards.IndexOf(card);
            Cards.RemoveAt(takedCardPosition);
            card.transform.SetParent(CursorContainerCenter);
            card.SetTargetPos(new Vector2(120, -10));
        }
        TakedCard = card;
        plantPlacer.SetTakedCard(TakedCard);
        CheckCardPosition(true);

        Aim.singleton.SwitchIsVisible(true);

        return true;
    }

    public void UseStarted()
    {
    }

    public void UseCanceled()
    {
        if (InputManager.singleton.UseTime >= 0.2f)
        {
            return;
        }

        var isHaveEmptySpace = isInserting && TakedCard != null && positionNow >= 0 && positionNow <= Cards.Count;
        if (isHaveEmptySpace)
        {
            PutCardBack(TakedCard, positionNow);
        }
        else {
            if (TakedCard != null && !InputManager.singleton.IsMouseOnGUI) {
                if (plantPlacer.TrySetPlant(TakedCard)) {
                    placedPlants += 1;
                    RemoveCardFromHand();
                }
            }
        }
    }

    public void AlternativeUseCanceled() {
        if (TakedCard != null)
        {
            if (InputManager.singleton.AlternativeUseTime < 0.2f)
            {
                PutCardBack(TakedCard, takedCardPosition);
            }
        }
    }

    public void PutCardBack(Card card, int inventoryPosition)
    {
        if (Cards.Contains(card))
            return;

        PutCardToInventory(card, inventoryPosition);
        TakedCard = null;
        plantPlacer.SetTakedCard(null);
        takedCardPosition = -1;
        CheckCardPosition(true);

        Aim.singleton.SwitchIsVisible(false);
    }

    public void PutCardToInventory(Card card, int inventoryPosition)
    {
        if (Cards.Contains(card))
            return;

        inventoryPosition = Mathf.Clamp(inventoryPosition, 0, Cards.Count);
        Cards.Insert(inventoryPosition, card);
        card.transform.SetParent(transform);
        CheckCardPosition(true);
    }

    public void RemoveCardFromHand() {
        Destroy(TakedCard.gameObject);
        TakedCard = null;
        plantPlacer.SetTakedCard(null);
        RecalculateCardPositions();
    }

    public void RemoveCardFromHandExited()
    {
        if(TakedCard != null)
            Destroy(TakedCard.gameObject);
        TakedCard = null;
    }

    public void GiveCardsPack()
    {
        PlantDataGenerator.singleton.AddPackIndex();

        if (PlantDataGenerator.singleton.GetPackIndex() % AlpacasManager.PacksToAlpacs == 0 && 
            AlpacasManager.singleton.AlpacasCountNow < AlpacasManager.MaxAlpacasCount)
        {
            var color1 = (AlpacaColor)Random.Range(0, 3);
            var color2 = (AlpacaColor)Random.Range(0, 3);
            CardPack.singleton.SetAlpacasToPack(0, true, color1);
            CardPack.singleton.SetAlpacasToPack(1, true, color2);
        }
        else
        {
            CardPack.singleton.SetAlpacasToPack(0, false, AlpacaColor.Red);
            CardPack.singleton.SetAlpacasToPack(1, false, AlpacaColor.Red);
        }
        CardPack.singleton.SetCardsToPack(0, SpawnCards());
        CardPack.singleton.SetCardsToPack(1, SpawnCards());

        spawnedCardsCount += 3;
    }

    public List<Card> SpawnCards()
    {
        var cards = new List<Card>();
        for (var i = 0; i < 3; i++)
        {
            var card = CreateNewCard(PlantDataGenerator.singleton.CreatePlantData());
            cards.Add(card);
            spawnedCards.Add(card);
        }

        return cards;
    }

    public Card CreateNewCard(PlantData plantData) {
        var cardObj = Instantiate(CardPrefab, transform);
        cardObj.transform.position = CardSpawner.position;

        var card = cardObj.GetComponent<Card>();
        card.plantData = plantData;

        return card;
    }

    public void RemoveCardFromSpawnedCards(Card card) {
        spawnedCards.Remove(card);
    }

    public void UpdateValuesOnSpawnedCards() {
        foreach (var card in spawnedCards) {
            card.UpdateValueOnCard();
        }
    }
}

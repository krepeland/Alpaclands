using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct SpawnPosition {
    public List<RectTransform> Postions;
}

public class SpawnedPack : CustomButton
{
    public List<Card> cards;
    [SerializeField] private Transform owner;

    public bool Locked;

    [SerializeField] List<SpawnPosition> Positions;
    [SerializeField] int PackId;

    [SerializeField] MeshRenderer AlpacaMeshRenderer;
    [SerializeField] MeshRenderer AlpacaMeshRenderer2;
    [SerializeField] Material[] AlpacaColorsMaterials;

    private bool hasAlpaca;
    private AlpacaColor alpacaColor;

    public bool isResizing;
    public float nowX;
    public float targetX;
    float t;

    private void Awake()
    {
        SetIsVisible(false);
        owner.transform.localScale = new Vector3(0, 1, 1);
    }

    private void Update()
    {
        if (!isResizing)
            return;

        if (t > 0)
        {
            t -= Time.deltaTime;
            return;
        }
        nowX = Mathf.Clamp01(Mathf.Lerp(nowX, targetX, Time.deltaTime / Mathf.Abs(nowX - targetX) * 5));
        if (nowX == targetX)
        {
            isResizing = false;
        }
        Resizing();
    }
    void Resizing()
    {
        owner.transform.localScale = new Vector3(nowX, 1, 1);
        UpdateCardPoses();
    }

    public void SetIsVisible(bool isVisible) {
        Locked = !isVisible;
        targetX = isVisible ? 1 : 0;
        isResizing = true;
        if (isVisible)
        {
            owner.gameObject.SetActive(true);
        }
        else {
            AlpacaMeshRenderer2.gameObject.SetActive(false);
        }
    }

    public void SetCards(List<Card> newCards) {
        if (cards.Count != 0) {
            DestroyCards();
        }

        cards = newCards;
        UpdateCardPoses();

        SetIsVisible(true);
    }

    public void SetAlpaca(bool hasAlpaca, AlpacaColor alpacaColor)
    {
        this.hasAlpaca = hasAlpaca;
        AlpacaMeshRenderer2.gameObject.SetActive(hasAlpaca);
        AlpacaMeshRenderer.material = AlpacaColorsMaterials[(int)alpacaColor];
        this.alpacaColor = alpacaColor;

        UpdateCardPoses();

        SetIsVisible(true);
    }

    void UpdateCardPoses()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.IsInPack = true;
            card.transform.localScale = Vector3.one;
            card.transform.SetParent(owner);
            card.transform.SetSiblingIndex(0);
            card.transform.localRotation = Quaternion.Euler(0, 0, 0);
            card.transform.position = Positions[cards.Count - 1].Postions[i].position;
        }
    }

    public void DestroyCards()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }
        cards = new List<Card>();
    }

    public override void OnHover()
    {
        if (Locked)
            return;
        owner.localScale = Vector3.one * 1.05f;
        base.OnHover();
    }

    public override void OutHover()
    {
        if (Locked)
            return;
        owner.localScale = Vector3.one;
        base.OutHover();
    }

    public override void OnClicked(int mouseButton)
    {
        if (Locked)
            return;
        owner.localScale = Vector3.one;
        Locked = true;
        StartCoroutine(GiveCards());
        CardPack.singleton.PackSelected(PackId);

        if (hasAlpaca)
        {
            AlpacasManager.singleton.SpawnAlpaca(alpacaColor);
        }
    }

    IEnumerator GiveCards() {
        foreach (var e in cards) {
            CardManager.singleton.PutCardToInventory(e, 0);
            e.transform.localRotation = Quaternion.Euler(0, 0, 0);
            e.transform.localScale = Vector3.one;
            e.IsInPack = false;
            yield return new WaitForSeconds(0.1f);
        }

        Locked = false;
        cards = new List<Card>();
        CardPack.singleton.CardsGiven(PackId);
    }
}

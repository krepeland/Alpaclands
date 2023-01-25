using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPack : CustomButton
{
    public int countNow;

    public static CardPack singleton;

    [SerializeField] private Text CountText1;
    [SerializeField] private Text CountText2;

    public bool isResizing;
    public float nowX;
    public float targetX;

    [SerializeField] private SpawnedPack pack1;
    [SerializeField] private SpawnedPack pack2;

    public bool isSelecting;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        CountUpdated();

        nowX = 0;
        Resizing();
    }

    public void SetAlpacasToPack(int pack, bool hasAlpaca, AlpacaColor alpacaColor)
    {
        if (pack == 0)
        {
            pack1.SetAlpaca(hasAlpaca, alpacaColor);
        }
        else
        {
            pack2.SetAlpaca(hasAlpaca, alpacaColor);
        }
        isSelecting = true;
    }

    public void SetCardsToPack(int pack, List<Card> cards) {
        if (pack == 0)
        {
            pack1.SetCards(cards);
        }
        else
        {
            pack2.SetCards(cards);
        }
        isSelecting = true;
    }

    public void PackSelected(int pack)
    {
        if (pack == 0)
        {
            pack2.SetIsVisible(false);
        }
        else
        {
            pack1.SetIsVisible(false);
        }
    }

    public void CardsGiven(int pack)
    {
        if (pack == 0)
        {
            pack1.SetIsVisible(false);
        }
        else
        {
            pack2.SetIsVisible(false);
        }
        isSelecting = false;
        ProgressBar.singleton.CheckIsGameEnded();
    }

    public void Exited() {
        pack1.SetIsVisible(false);
        pack2.SetIsVisible(false);
        isSelecting = false;
    }

    public void AddToCount() {
        countNow += 1;
        CountUpdated();
    }

    public override void OnClicked(int mouseButton)
    {
        if (isSelecting)
            return;
        if (countNow <= 0)
            return;
        countNow -= 1;
        CountUpdated();
        CardManager.singleton.GiveCardsPack();
    }

    private void Update()
    {
        if (!isResizing)
            return;

        nowX = Mathf.Clamp01(Mathf.Lerp(nowX, targetX, Time.deltaTime / Mathf.Abs(nowX - targetX) * 5));
        if (nowX == targetX)
        {
            isResizing = false;
        }
        Resizing();
    }

    void Resizing()
    {
        transform.localScale = IsHovered ? new Vector3(nowX * 1.1f, 1.1f, 1) : new Vector3(nowX, 1, 1);
    }

    void CountUpdated() {
        if (countNow <= 0)
        {
            isResizing = true;
            targetX = 0;
        }
        else
        {
            isResizing = true;
            targetX = 1;
        }
        if (countNow < 2)
        {
            CountText1.enabled = false;
            CountText2.enabled = false;
        }
        else
        {
            CountText1.enabled = true;
            CountText2.enabled = true;
        }
        CountText1.text = $"x{countNow}";
        CountText2.text = $"x{countNow}";
    }

    public override void OnHover()
    {
        Resizing();
    }

    public override void OutHover()
    {
        Resizing();
    }

}

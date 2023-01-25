using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGetter : MonoBehaviour
{

    private float size;
    private bool isResizing;
    float t;

    public bool isHidden;
    private float targetPos;

    [SerializeField] private Text text1;
    [SerializeField] private Text text2;
    [SerializeField] private RectTransform rect;
    float timeTillHide;

    public static MoneyGetter singleton;

    [SerializeField] private Transform canvas;
    [SerializeField] private Transform coinUIPrefab;

    public void Show(float tTillHide = 2) {
        isHidden = false;
        timeTillHide = tTillHide;
    }

    private void Awake()
    {
        singleton = this;
        UpdateText();
        rect.anchoredPosition = new Vector2(0, 75);
    }

    public void SpawnCoin(Vector3 worldPos, float speed = 1) {
        var ray = new Ray(Camera.main.transform.position, worldPos - Camera.main.transform.position);
        var pos = ray.GetPoint(1.5f);
        var coin = Instantiate(coinUIPrefab, canvas);
        coin.transform.position = pos;
        coin.GetComponent<TakedCoin>().TargetObject = transform;
        coin.GetComponent<TakedCoin>().speed = speed;
    }

    public void GetCoin() {
        KeyManager.AddToKey("Coins", 1);
        if (KeyManager.GetKey("Coins", 0) % 10 == 0) {
            AchievmentsManager.Achievment_CheckCoins();
        }
        UpdateText();
        timeTillHide = 1f;
        isHidden = false;
        Resize();
    }

    void UpdateText() {
        var coins = KeyManager.GetKey("Coins").ToString();

        text1.text = coins;
        text2.text = coins;
    }

    void Resize() {
        isResizing = true;
        t = 0;
        size = 1;
    }

    void Update()
    {
        if (!isHidden)
        {
            targetPos = -75;
            timeTillHide -= Time.deltaTime;
            if (timeTillHide <= 0)
            {
                timeTillHide = 0;
                isHidden = true;
            }
        }
        else
        {
            targetPos = 75;
        }

        if (Mathf.Abs(rect.anchoredPosition.y - targetPos) > 0.5f) {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, targetPos), Time.deltaTime * 5);
        }

        if (!isResizing)
            return;
        t += Time.deltaTime * 5;

        size = Mathf.Clamp(1 + t * 0.2f, 1, 1.1f);
        if (t >= 1) {
            isResizing = false;
            size = 1;
        }

        transform.localScale = new Vector3(size, size, 1);
    }
}

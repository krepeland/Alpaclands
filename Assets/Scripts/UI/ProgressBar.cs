using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int TargetPoints = 10;
    public int StartPoints = 0;
    public int PointsPerLevel = 5;
    public int MaxPointsPerLevel = 50;
    public int PointsNow;
    public int PointsGet;

    [SerializeField] private Image ProgressNext;
    [SerializeField] private Image ProgressNow;
    [SerializeField] private Text PointsText;
    [SerializeField] private Text PointsText2;
    public Transform TargetPos;
    [SerializeField] private GameObject PointPrefab;

    public static ProgressBar singleton;

    private int spawnedPointsCount;

    public float T;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        UpdatePointsText();
    }

    public void SetPointsGet(int pointsGet) {
        PointsGet = pointsGet;
        ProgressNext.fillAmount = ((pointsGet + PointsNow - StartPoints) / (float)(TargetPoints - StartPoints));
    }

    public void UpdatePointsNow()
    {
        ProgressNow.fillAmount = ((PointsNow - StartPoints) / (float)(TargetPoints - StartPoints));
    }

    public void AddPoints(int points, Transform spawner)
    {
        spawnedPointsCount += points;
        StartCoroutine(SpawnPoints(points, spawner));
    }

    public IEnumerator SpawnPoints(int count, Transform spawner) {
        float T = Mathf.Clamp(0.01f, 0.1f, 0.5f / count);
        int countPerTime = 1;

        if (count > 5) {
            countPerTime = count / 20;
        }
        if (countPerTime <= 0)
            countPerTime = 1;

        while (count > 0)
        {
            for (var j = 0; j < countPerTime; j++)
            {
                if (count <= 0)
                    break;

                count -= 1;
                var pos = Camera.main.WorldToScreenPoint(spawner.position);
                var point = Instantiate(PointPrefab, transform);
                //point.GetComponent<RectTransform>().anchoredPosition = pos;
                point.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 1f));
            }
            yield return new WaitForSeconds(T);
        }
    }

    public void PointGetted()
    {
        spawnedPointsCount -= 1;
        PointsNow += 1;

        if (PointsNow % 50 == 0)
        {
            KeyManager.SetKey("Score", PointsNow);
            AchievmentsManager.Achievment_CheckScore();
        }

        if (PointsNow >= TargetPoints) {
            StartPoints = TargetPoints;
            PointsPerLevel += 5;
            //if (PointsPerLevel > MaxPointsPerLevel)
            //    PointsPerLevel = MaxPointsPerLevel;
            TargetPoints += PointsPerLevel;
            CardPack.singleton.AddToCount();
        }

        if (spawnedPointsCount <= 0)
        {
            CheckIsGameEnded();
        }

        UpdatePointsText();
        UpdatePointsNow();
    }

    void UpdatePointsText() {
        PointsText.text = $"{PointsNow}/{TargetPoints}";
        PointsText2.text = PointsText.text;
    }

    public void CheckIsGameEnded()
    {
        if (CardPack.singleton.countNow > 0 || 
            CardManager.singleton.Cards.Count > 0 || 
            spawnedPointsCount > 0 || 
            CardPack.singleton.isSelecting ||
            CardManager.singleton.TakedCard != null)
        {
            return;
        }
        LevelEndCard.singleton.LevelEnded();

    }

    private void Update()
    {
        if (!LevelEndCard.singleton.ended)
        {
            T += Time.deltaTime;
        }
    }
}

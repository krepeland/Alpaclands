using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreToCoins : MonoBehaviour
{
    float T;
    int Count;
    int Score;

    [SerializeField] private Text Score1text;
    [SerializeField] private Text Score2text;

    [SerializeField] private Transform TargetPos;


    private void Start()
    {
        Score = ProgressBar.singleton.PointsNow;
        Score1text.text = Score.ToString();
        Score2text.text = Score.ToString();
        StartCoroutine(GetCoins());
        TakeableCoin.TakeAllCoins();
    }

    IEnumerator GetCoins()
    {
        MoneyGetter.singleton.isHidden = false;
        yield return new WaitForSeconds(1.5f);
        while (Score >= 100)
        {
            Score -= 100;
            //MoneyGetter.singleton.GetCoin();
            MoneyGetter.singleton.SpawnCoin(transform.position);
            Score1text.text = Score.ToString();
            Score2text.text = Score.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void Update()
    {
        T += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, TargetPos.transform.position, Time.deltaTime / Vector3.Distance(transform.position, TargetPos.transform.position));
    }
}

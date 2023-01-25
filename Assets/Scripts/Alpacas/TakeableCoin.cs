using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableCoin : MonoBehaviour
{
    float t;
    Vector3 startPos;
    Vector3 offset;
    [SerializeField] float height = 0.5f;

    private static List<TakeableCoin> coins;

    public static void ReloadCoinsSet() {
        coins = new List<TakeableCoin>();
    }

    public static void TakeAllCoins(float speed = 1) {
        for (var i = 0; i < coins.Count; i++) {
            if (coins[i] != null)
            {
                coins[i].Taked(speed);
            }
        }
        coins = new List<TakeableCoin>();
    }

    private void Start()
    {
        startPos = transform.position;
        offset = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        coins.Add(this);
    }

    public void Update()
    {
        t += Time.deltaTime;

        transform.position = startPos + new Vector3(Mathf.Lerp(0, offset.x, t), height / 4f + -height * (t - 0.5f) * (t - 0.5f), Mathf.Lerp(0, offset.z, t));

        if (t >= 1f)
        {
            enabled = false;
            GetComponent<Animator>().enabled = true;      
            transform.position = startPos + offset;
        }
    }

    public void Taked(float speed = 1)
    {
        MoneyGetter.singleton.SpawnCoin(transform.position, speed);
        Destroy(gameObject);
    }
}

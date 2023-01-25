using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedCoin : MonoBehaviour
{
    public Transform TargetObject;
    private RectTransform rect;
    public float speed = 1;

    public AudioClip takedCoin;

    private void Start()
    {
        MoneyGetter.singleton.Show();

        SoundManager.TryPlaySound(takedCoin);
    }

    void Update()
    {
        var dist = Vector2.Distance(transform.position, TargetObject.position);

        if (dist > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, TargetObject.position, Time.deltaTime / dist * speed);
        }
        else {
            Destroy(gameObject);
            MoneyGetter.singleton.GetCoin();
        }
    }
}

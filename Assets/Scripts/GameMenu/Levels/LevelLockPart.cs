using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLockPart : MonoBehaviour
{
    float T;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-3000f, 3000f), Random.Range(5000f, 15000f)));
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-150, 150));
    }

    private void Update()
    {
        T += Time.deltaTime;
        if (T >= 5f)
            Destroy(gameObject);
    }
}

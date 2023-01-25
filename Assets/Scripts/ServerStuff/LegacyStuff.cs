using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyStuff : MonoBehaviour
{

    private void Start()
    {
        if (KeyManager.GetKey("Legacy", -1) != -1) {
            Destroy(gameObject);
        }
    }

    public void SetIsAccepted(bool isAccepted) {
        KeyManager.SetKey("Legacy", isAccepted ? 1 : 0);
        Destroy(gameObject);
    }
}

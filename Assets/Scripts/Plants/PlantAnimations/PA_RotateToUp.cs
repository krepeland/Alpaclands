using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_RotateToUp : MonoBehaviour
{
    [SerializeField] float MaxAngle;

    void Start()
    {
        var resultX = transform.localEulerAngles.x;

        if (resultX > MaxAngle)
            resultX = MaxAngle;

        transform.localRotation = Quaternion.Euler(
            resultX, 
            transform.localEulerAngles.y, 
            transform.localEulerAngles.z
            );    
    }
}

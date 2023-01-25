using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_RandomRotation : MonoBehaviour
{
    public float MinAngle = 0;
    public float MaxAngle = 360;
    public Vector3 Axis = new Vector3(0, 1, 0);
    void Start()
    {
        var AddRotation = Axis * Random.Range(MinAngle, MaxAngle);
        transform.localRotation = Quaternion.Euler(
            transform.localEulerAngles.x + AddRotation.x,
            transform.localEulerAngles.y + AddRotation.y,
            transform.localEulerAngles.z + AddRotation.z);

        Destroy(this);
    }
}

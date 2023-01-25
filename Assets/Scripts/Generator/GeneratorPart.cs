using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorPart : MonoBehaviour
{
    public Vector3 MinAngles = new Vector3(0, 0, 0);
    public Vector3 MaxAngles = new Vector3(360, 360, 360);

    public Vector3 MinScaleMultiply = new Vector3(1, 1, 1);
    public Vector3 MaxScaleMultiply = new Vector3(1, 1, 1);

    public Vector3 MinOffset = new Vector3(0, 0, 0);
    public Vector3 MaxOffset = new Vector3(0, 0, 0);
}

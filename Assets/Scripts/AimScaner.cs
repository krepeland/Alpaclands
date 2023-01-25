using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScaner : MonoBehaviour
{
    Vector3 oldPos;

    public Color Color;

    public GameObject R;
    public GameObject G;
    public GameObject B;

    public UITriangle UITriangle;
    public Vector3 EnvCoordinateValues;
    public static AimScaner singleton;

    private void Awake()
    {
        singleton = this;
    }

    //void Update()
    //{
    //    if (transform.position == oldPos)
    //        return;
    //
    //    oldPos = transform.position;
    //    Scan();
    //}

    public void Scan() {
        var result = EnvironmentScaner.GetEnvironmentAtPoint(transform.position);
        //Debug.Log(result);

        var triangle = new EnvironmentTriangle(transform.position, result);
        Color = triangle.Color;

        var newV = triangle.ColorValues;
        var sumV = newV.x + newV.y + newV.z;
        newV /= sumV;

        EnvCoordinateValues = newV;
        UITriangle.SetValues(newV);

        R.transform.localPosition = new Vector3(0, 0, 1.732051f * newV.x);
        G.transform.localPosition = new Vector3(0, 0, 1.732051f * newV.y);
        B.transform.localPosition = new Vector3(0, 0, 1.732051f * newV.z);
    }
}

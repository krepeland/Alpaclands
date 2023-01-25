using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITriangle : MonoBehaviour
{
    [SerializeField] private RectTransform AimOnTriangle;
    public Vector2 AimOnTrianglePos;

    private void Awake()
    {
        CreateMesh();
    }

    public void CreateMesh()
    {
        var mesh = new Mesh();
        var colors = new Color[4] { Color.red, Color.green, Color.blue, new Color(0.7f, 0.7f, 0.7f) };
        var positions = new Vector3[4] { new Vector3(-0.8660254f, 0, -0.5f), new Vector3(0, 0, 1), new Vector3(0.8660254f, 0, -0.5f), Vector3.zero };
        var normals = new Vector3[4] { new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0) };

        mesh.SetVertices(positions);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
        mesh.triangles = new int[] { 0, 1, 3, 0, 3, 2, 3, 1, 2 };
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public static Vector2 GetCoordsOfEnvValue(Vector3 envValues)
    {
        var maxValue = Mathf.Max(envValues.x, envValues.y, envValues.z);
        var ColorValues = envValues / maxValue;

        var newV = ColorValues;
        var sumV = newV.x + newV.y + newV.z;
        newV /= sumV;
        return GetCoordsOfValue(newV);

    }

    public static Vector2 GetCoordsOfValue(Vector3 colorValues) {
        var rPos = Vector2.Lerp(new Vector2(0, 1), new Vector2(-0.8660254f, -0.5f), colorValues.x);
        var gPos = Vector2.Lerp(new Vector2(0.8660254f, -0.5f), new Vector2(0, 1), colorValues.y);

        var k1 = -1.732050f;

        var x = (k1 * rPos.x - rPos.y + gPos.y) / k1;
        var y = gPos.y;
        return new Vector2(100 * x, 100 * y - 26.5f);

    }

    public void SetValues(Vector3 colorValues)
    {
        AimOnTrianglePos = GetCoordsOfValue(colorValues);
        AimOnTriangle.anchoredPosition = AimOnTrianglePos;
    }
}

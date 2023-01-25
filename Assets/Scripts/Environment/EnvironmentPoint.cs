using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPoint : MonoBehaviour
{
    Mesh mesh;
    Color[] colors;
    Vector3[] values;
    Vector3[] positions;

    void Start()
    {
        var localToWorld = transform.localToWorldMatrix;

        mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;

        positions = new Vector3[vertices.Length];
        values = new Vector3[vertices.Length];
        colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            values[i] = new Vector3(5, 5, 5);
            positions[i] = localToWorld.MultiplyPoint3x4(vertices[i]);
        }
        UpdateColors();
    }

    public void RegisterAffector(EnvironmentAffector affector)
    {
        //for (int i = 0; i < colors.Length; i++)
        //{
        //    var power = affector.GetAffectPower(positions[i]);
        //    if (power <= 0)
        //        continue;
        //
        //    values[i] += power * affector.environmentTriangle.EnvironmentValues;
        //}
        //
        //UpdateColors();
        StartCoroutine(Recolor(affector, 10));
    }

    IEnumerator Recolor(EnvironmentAffector affector, int recolorIterations = 10)
    {
        recolorIterations = Mathf.Clamp(recolorIterations, 1, 10);
        var affectValues = affector.environmentTriangle.EnvironmentValues;
        var RecolorIterationsB = recolorIterations + 1;
        var list = new List<int>[RecolorIterationsB];
        for (var i = 0; i < RecolorIterationsB; i++) {
            list[i] = new List<int>();
        }

        for (var i = 0; i < colors.Length; i++)
        {
            var dist = Vector3.Distance(positions[i], affector.transform.position) * recolorIterations;
            if (dist <= 0)
                continue;

            var index = dist / affector.Radius;
            if (index > recolorIterations)
                continue;

            list[(int)(index)].Add(i);
        }

        for (var i = 0; i < RecolorIterationsB; i++)
        {
            for (var j = 0; j < i; j++)
            {
                foreach (var e in list[j])
                {
                    var power = affector.GetAffectPower(positions[e]);
                    if (power <= 0)
                        continue;

                    values[e] += (1f / (float)(recolorIterations - j)) * power * affectValues;
                    UpdateColor(e);
                }
            }
            UpdateMeshColors();
            yield return new WaitForSeconds(0.05f);
        }
    }

    void UpdateColor(int i)
    {
        var value = values[i];
        var maxValue = Mathf.Max(value.x, value.y, value.z);
        var newValue = value / maxValue;
        var color = new Color(newValue.x, newValue.y, newValue.z);
        colors[i] = color;
    }

    void UpdateColors()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            var value = values[i];
            var maxValue = Mathf.Max(value.x, value.y, value.z);
            var newValue = value / maxValue;
            var color = new Color(newValue.x, newValue.y, newValue.z);
            colors[i] = color;
        }
        UpdateMeshColors();
    }

    void UpdateMeshColors()
    {
        mesh.colors = colors;
    }
}

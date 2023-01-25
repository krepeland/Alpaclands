using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private EnvironmentAffector environmentAffector;
    public Vector3 plantColors;
    [SerializeField] private Material material;
    private Material newMaterial;
    [SerializeField] private List<MeshRenderer> renderers;
    [SerializeField] private List<GameObject> objectsToDeleteAfterCombine;

    public Transform debugTestSpherePrefab;

    public void Initialize(PlantData plantData)
    {
        environmentAffector.Initialize(plantData);
        environmentAffector.environmentTriangle.EnvironmentValues = plantData.EnvironmentValues;

        environmentAffector.environmentTriangle.EnvironmentValuesUpdated();
        plantColors = environmentAffector.environmentTriangle.EnvironmentValues + Vector3.one * 5;

        newMaterial = new Material(material);

        foreach (var e in renderers)
        {
            e.material = newMaterial;
        }
        GetComponent<MeshRenderer>().material = newMaterial;

        RecalculateTriangle();

        Affect();
    }

    public void CombineFilters()
    {
        CombineInstance[] combine = new CombineInstance[renderers.Count];
        int i = 0;
        while (i < renderers.Count)
        {
            var filter = renderers[i].GetComponent<MeshFilter>();
            combine[i].mesh = filter.sharedMesh;
            combine[i].transform = transform.worldToLocalMatrix * filter.transform.localToWorldMatrix;
            filter.gameObject.SetActive(false);

            i++;
        }
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        foreach (var e in objectsToDeleteAfterCombine) {
            Destroy(e);
        }
        objectsToDeleteAfterCombine = null;
        renderers = null;
    }

    void Affect() {
        environmentAffector.Affect();
        var affectAt = EnvironmentScaner.GetEnvironmentAffectors(transform.position, environmentAffector.Radius);

        foreach (var e in affectAt) {
            if (e == environmentAffector)
                continue;

            var power = environmentAffector.GetAffectPower(e.transform.position);
            if (power <= 0)
                continue;

            var sphere = Instantiate(debugTestSpherePrefab, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<PlantColorSphere>();
            sphere.SetTarget(
                e.GetPlant(), 
                e.transform.position, 
                environmentAffector.GetEnvironmentAffect(e.transform.position),
                environmentAffector.environmentTriangle.Color,
                power);
        }

        var getAffectedBy = EnvironmentScaner.GetEnvironmentAffectors(transform.position, 0.5f);

        foreach (var e in getAffectedBy)
        {
            if (e == environmentAffector)
                continue;

            var power = e.GetAffectPower(transform.position);
            if (power <= 0)
                continue;

            var sphere = Instantiate(debugTestSpherePrefab, e.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<PlantColorSphere>();
            sphere.SetTarget(
                this,
                transform.position,
                e.GetEnvironmentAffect(transform.position),
                e.environmentTriangle.Color,
                power);
        }
    }

    public void GetAffectValue(Vector3 values)
    {
        plantColors += values;
        RecalculateTriangle();
    }

    void RecalculateTriangle() {
        var triangle = new EnvironmentTriangle(transform.position, plantColors);
        newMaterial.SetColor("Color_87f55049ed374f2cacaf8edfa791e5bc", triangle.Color);
    }
}

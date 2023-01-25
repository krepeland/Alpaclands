using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAffector : MonoBehaviour
{
    public EnvironmentTriangle environmentTriangle;
    [SerializeField] private Plant plant;

    public float MinRadius = 0.2f;
    public float Radius = 1;

    float K;
    float C;

    public void Initialize(PlantData plantData)
    {
        MinRadius = plantData.MinRadius;
        Radius = plantData.Radius;

        environmentTriangle.CenterPos = transform.position;

        K = 1 / (Radius - MinRadius);
        C = Radius / (Radius - MinRadius);

        var col = gameObject.AddComponent<SphereCollider>();
        col.radius = Radius;
        col.isTrigger = true;
    }

    public float GetAffectPower(Vector3 point) {
        return Mathf.Clamp01(-K * Vector3.Distance(point, environmentTriangle.CenterPos) + C);
    }

    public Vector3 GetEnvironmentAffect(Vector3 point)
    {
        return environmentTriangle.EnvironmentValues * GetAffectPower(point);
    }

    public void Affect() {
        foreach (var point in EnvironmentScaner.GetEnvironmentPoints(transform.position, Radius)) {
            point.RegisterAffector(this);
        }
    }

    public Plant GetPlant() {
        return plant;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = environmentTriangle.Color;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

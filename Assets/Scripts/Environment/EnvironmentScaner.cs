using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentScaner
{
    public static HashSet<EnvironmentPoint> GetEnvironmentPoints(Vector3 point, float radius) {
        var allPoints = Physics.SphereCastAll(point, radius, new Vector3(0, 0.1f, 0));

        var result = new HashSet<EnvironmentPoint>();
        foreach (var e in allPoints)
        {
            if (e.collider.gameObject.CompareTag("EnvironmentPoint"))
            {
                result.Add(e.collider.GetComponent<EnvironmentPoint>());
            }
        }

        return result;
    }

    public static HashSet<EnvironmentAffector> GetEnvironmentAffectors(Vector3 point, float radius)
    {
        var allPoints = Physics.SphereCastAll(point, radius, new Vector3(0, 0.1f, 0));

        var result = new HashSet<EnvironmentAffector>();
        foreach (var e in allPoints)
        {
            if (e.collider.gameObject.CompareTag("EnvironmentAffector"))
            {
                result.Add(e.collider.GetComponent<EnvironmentAffector>());
            }
        }

        return result;
    }

    public static Vector3 GetEnvironmentAtPoint(Vector3 point)
    {
        HashSet<EnvironmentAffector> affectors = GetEnvironmentAffectors(point, 0.1f);
        Vector3 result = Vector3.zero;

        foreach (var e in affectors) {
            result += e.GetEnvironmentAffect(point);
        }

        return result;
    }
}

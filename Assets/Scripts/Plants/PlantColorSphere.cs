using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantColorSphere : MonoBehaviour
{
    Plant target;
    Vector3 targetPos;
    Vector3 affectValues;
    bool isMoving;
    [SerializeField] MeshRenderer sphereRenderer;
    [SerializeField] private Material material;

    float height;
    float fullDistance;

    private void Start()
    {
        
    }

    public void SetTarget(Plant target, Vector3 targetPos, Vector3 affectValues, Color color, float size)
    {
        sphereRenderer.material = new Material(material);
        sphereRenderer.material.SetColor("Color_87f55049ed374f2cacaf8edfa791e5bc", color);

        sphereRenderer.transform.localScale *= Mathf.Clamp01(size * 2);

        this.target = target;
        this.targetPos = targetPos;
        this.affectValues = affectValues;
        isMoving = true;

        fullDistance = Vector3.Distance(transform.position, targetPos);
        height = (fullDistance + Random.Range(0f, fullDistance / 2f)) * 6;
    }

    void Update()
    {
        if (!isMoving)
            return;
        var dist = Vector3.Distance(transform.position, targetPos);
        var value = Time.deltaTime / dist * 3;
        if (value < 1)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, value);
        }
        else {
            isMoving = false;
            target.GetAffectValue(affectValues);
            Destroy(gameObject);
        }

        var part = dist / fullDistance;
        sphereRenderer.transform.localPosition = new Vector3(0, height/4f + -height * (part - 0.5f) * (part - 0.5f));
    }
}

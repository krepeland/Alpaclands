using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public static Aim singleton;
    [SerializeField] Transform AimTransform;
    [SerializeField] GameObject AimRender;
    [SerializeField] Camera gameCamera;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask coinMask;
    [SerializeField] LayerMask layerMaskPlantCollider;

    public Transform Spawner;
    public Vector3 PointPosition;
    public Vector3 NormalAtPosition;
    
    public bool IsVisible;
    public bool IsPlaceSuitable;
    public bool IsHit;

    private PlantPlacer plantPlacer;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        plantPlacer = PlantPlacer.singleton;
    }

    private void Update()
    {
        UpdateAim(InputManager.singleton.MousePosition);
    }

    public void SwitchIsVisible(bool isVisible) {
        IsVisible = isVisible;
        CheckIsVisible();
    }

    public bool CheckIsVisible()
    {
        var result = IsVisible && IsHit && IsPlaceSuitable;
        AimRender.SetActive(result);
        return result;
    }

    void UpdateAim(Vector2 mousePos)
    {
        var ray = gameCamera.ScreenPointToRay(mousePos);

        var isHitNew = Physics.SphereCast(ray, 0.1f, out var hit, 100, layerMask);

        if (isHitNew)
        {
            PointPosition = hit.point;
            NormalAtPosition = hit.normal;

            AimTransform.position = PointPosition;
            AimTransform.LookAt(PointPosition + NormalAtPosition);
        }

        if (Physics.SphereCast(ray, 0.1f, out var hit2, 100, coinMask))
        {
            hit2.collider.gameObject.GetComponent<TakeableCoin>().Taked();
        }

        if (AimTransform.position.y < 0.3f)
        {
            IsPlaceSuitable = false;
        }
        else
        {
            if (Physics.OverlapSphere(PointPosition, 0.05f, layerMaskPlantCollider).Length > 0) {
                IsPlaceSuitable = false;
            }
            else
            {
                IsPlaceSuitable = true;
            }
        }

        if (isHitNew != IsHit)
        {
            IsHit = isHitNew;
        }
        CheckIsVisible();
        plantPlacer.AimUpdated();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform cameraTransform;
    [SerializeField] public Camera gameCamera;
    [SerializeField] private GraphicRaycasterUI raycasterUI;

    [Space(20)]
    [SerializeField] private float boostMultiplier = 2;

    private readonly float movementTime = 5f;

    //Rotation Y
    private float newAngleY;
    private float nowAngleY;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float rotationMouseSencivity = 180;
    //

    //Move
    [SerializeField] private float moveSpeed = 15;
    private Vector3 newPosition;
    [Space(20)]
    public float BorderX;
    public float BorderXMin;
    [Space(20)]
    public float BorderZ;
    public float BorderZMin;
    private bool MouseMoveLocked;
    //


    [SerializeField] private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;

    private Vector2 rotateStartPosition;
    private Vector2 rotateCurrentPosition;

    private Vector2 zoomStartPosition;
    private Vector2 zoomCurrentPosition;

    private InputManager inputManager;

    public static CameraController singleton;

    public bool LevelEnded;
    float levelEndT = 0f;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        inputManager = InputManager.singleton;

        newPosition = transform.position;

        newAngleY = transform.rotation.eulerAngles.y;
        nowAngleY = newAngleY;

        //zoomValue = cameraTransform.GetComponent<Camera>().fieldOfView;

        inputManager.UseStarted += () => { UseStarted(); };
        inputManager.UseCanceled += () => { UseCanceled(); };
    }

    private void Update()
    {
        if (LevelEnded)
        {
            UpdateLevelEnded();
            return;
        }
        float boostValue = inputManager.IsBoost ? boostMultiplier : 1;

        UpdateRotation(boostValue);
        UpdatePosition(boostValue);
    }

    void UpdateLevelEnded() {
        levelEndT = Mathf.Clamp(levelEndT + Time.deltaTime, 0, 3);
        transform.position += transform.right * Time.deltaTime * 10 * levelEndT;
    }

    public void UseStarted()
    {
        if (raycasterUI.IsMouseOnGUI)
        {
            MouseMoveLocked = true;
        }

        var plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        var ray = gameCamera.ScreenPointToRay(inputManager.MousePosition);
        float entery;

        if (plane.Raycast(ray, out entery))
        {
            dragStartPosition = ray.GetPoint(entery);
        }
    }

    public void UseCanceled()
    {
        MouseMoveLocked = false;
    }

    void UpdatePosition(float boostValue)
    {
        if (!MouseMoveLocked && inputManager.IsUse)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            Ray ray = gameCamera.ScreenPointToRay(inputManager.MousePosition);
            float entery;

            if (plane.Raycast(ray, out entery))
            {
                dragCurrentPosition = ray.GetPoint(entery);
                var temp = transform.position + dragStartPosition - dragCurrentPosition;
                newPosition = new Vector3(temp.x, newPosition.y, temp.z);
            }
        }

        newPosition += moveSpeed * boostValue * Time.deltaTime * (Quaternion.Euler(0, nowAngleY, 0) * new Vector3(inputManager.Move.x, 0, inputManager.Move.y));

        newPosition.x = Mathf.Clamp(newPosition.x, BorderXMin, BorderX);
        newPosition.z = Mathf.Clamp(newPosition.z, BorderZMin, BorderZ);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    void UpdateRotation(float boostValue) {

        newAngleY += inputManager.Rotate * rotationSpeed * Time.deltaTime * boostValue;

        if (inputManager.IsAlternativeUse)
        {
            newAngleY += inputManager.MouseDelta.x * rotationMouseSencivity * Time.deltaTime;
        }

        nowAngleY = Mathf.Lerp(nowAngleY, newAngleY, Time.deltaTime * movementTime);

        if (newAngleY > 7200f) {
            newAngleY -= 7200f;
            nowAngleY -= 7200f;
        }

        if (newAngleY < -7200f)
        {
            newAngleY += 7200f;
            nowAngleY += 7200f;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, nowAngleY, transform.rotation.eulerAngles.z);
    }
}

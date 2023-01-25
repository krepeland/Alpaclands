using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager singleton;
    private InputMap inputActions;

    public Vector2 Move;
    public float Rotate;

    public float Scroll;
    public float ScrollTime;

    public bool IsBoost;
    public float BoostTime;

    public bool IsUse;
    public float UseTime;

    public bool IsAlternativeUse;
    public float AlternativeUseTime;

    public Vector2 MousePosition;
    public Vector2 MouseDelta;

    public Action UseStarted;
    public Action UseClicked;
    public Action UseHold;
    public Action UseCanceled;

    public Action AlternativeUseStarted;
    public Action AlternativeUseClicked;
    public Action AlternativeUseHold;
    public Action AlternativeUseCanceled;
    public Action<Vector2> OnMouseDelta;
    public Action<Vector2> OnMousePosition;

    public bool IsMouseOnGUI;
    [SerializeField] private GraphicRaycasterUI raycasterUI;

    bool isSendedUseHolded;
    bool isSendedAltHolded;

    private void Awake()
    {
        singleton = this;

        inputActions = new InputMap();

        inputActions.Gameplay.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();

        inputActions.Gameplay.Move.canceled += ctx => Move = Vector2.zero;

        inputActions.Gameplay.Rotate.performed += ctx => Rotate = ctx.ReadValue<float>();

        inputActions.Gameplay.Rotate.canceled += ctx => Rotate = 0;

        inputActions.Gameplay.Scroll.performed += ctx => OnScroll(ctx.ReadValue<float>());
        inputActions.Gameplay.Scroll.canceled += ctx => OnScroll(0);

        inputActions.Gameplay.Boost.performed += ctx => IsBoost = true;
        inputActions.Gameplay.Boost.canceled += ctx => { IsBoost = false; BoostTime = 0; };

        inputActions.Gameplay.Use.started += ctx => UseStarted?.Invoke();
        inputActions.Gameplay.Use.performed += ctx => IsUse = true;
        inputActions.Gameplay.Use.canceled += ctx => { 
            IsUse = false;
            if (UseTime < 0.2f)
            {
                UseClicked?.Invoke();
            }
            UseCanceled?.Invoke();
            UseTime = 0;
            isSendedUseHolded = false;
        };

        inputActions.Gameplay.AlternativeUse.started += ctx => AlternativeUseStarted?.Invoke();
        inputActions.Gameplay.AlternativeUse.performed += ctx => IsAlternativeUse = true;
        inputActions.Gameplay.AlternativeUse.canceled += ctx => { 
            IsAlternativeUse = false;
            if (AlternativeUseTime < 0.2f)
            {
                AlternativeUseClicked?.Invoke();
            }
            AlternativeUseCanceled?.Invoke();
            AlternativeUseTime = 0; 
            isSendedAltHolded = false;
        };

        inputActions.Gameplay.MousePosition.performed += ctx => SetMousePosition(ctx.ReadValue<Vector2>());

        inputActions.Gameplay.MouseDelta.performed += ctx => SetMouseDelta(ctx.ReadValue<Vector2>());
        inputActions.Gameplay.MouseDelta.canceled += ctx => SetMouseDelta(Vector2.zero);

        inputActions.Gameplay.Something.canceled += ctx => SomethingTest();

        inputActions.Gameplay.Pause.started += ctx => SettingsManager.singleton.ChangeIsOpened();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void SomethingTest()
    {
        //ProgressBar.singleton.AddPoints(10, ProgressBar.singleton.transform);
    }

    private void OnScroll(float value) {
        Scroll = value;
        if (value == 0) {
            ScrollTime = 0;
        }
    }

    private void SetMousePosition(Vector2 mousePos) {
        MousePosition = mousePos;
        raycasterUI.RayCastToResults(MousePosition);
        OnMousePosition?.Invoke(mousePos);
    }

    private void SetMouseDelta(Vector2 mouseDelta)
    {
        MouseDelta = mouseDelta;
        OnMouseDelta?.Invoke(MouseDelta);
    }

    private void Update()
    {
        if (Scroll != 0)
            ScrollTime += Time.deltaTime;

        if (IsBoost)
            BoostTime += Time.deltaTime;

        if (IsUse)
        {
            UseTime += Time.deltaTime;

            if (!isSendedUseHolded && UseTime >= 0.2f)
            {
                UseHold?.Invoke();
                isSendedUseHolded = true;
            }
        }

        if (IsAlternativeUse)
        {
            AlternativeUseTime += Time.deltaTime;

            if (!isSendedAltHolded && AlternativeUseTime >= 0.2f)
            {
                AlternativeUseHold?.Invoke();
                isSendedAltHolded = true;
            }
        }
    }

    public void CheckIsStillOnGUI()
    {
        IsMouseOnGUI = raycasterUI.RayCastToResults(MousePosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public RectTransform CursorContainer;
    public RectTransform CursorContainerCenter;
    public Camera mainCamera;
    CustomButton selectedCustomButton;
    CameraController cameraController;
    InputManager inputManager;
    GraphicRaycasterUI graphicRaycasterUI;

    bool isButtonHold;
    bool isSendedHolded;
    int mouseButton;
    float buttonHoldTime;
    float timeToHold;

    public static UIManager singleton;

    public void Awake()
    {
        singleton = this;
        graphicRaycasterUI = GetComponent<GraphicRaycasterUI>();
    }

    private void Start()
    {
        cameraController = CameraController.singleton;
        inputManager = InputManager.singleton;
        mainCamera = Camera.main;

        inputManager.UseStarted += () => { UseStarted(); };
        inputManager.UseCanceled += () => { UseCanceled(); };
    }

    void Update()
    {
        UpdatedMousePosition(inputManager.MousePosition);
        if (isButtonHold)
        {
            buttonHoldTime += Time.deltaTime;
            if (!isSendedHolded && buttonHoldTime >= timeToHold)
            {
                selectedCustomButton?.CallOnHold(mouseButton);
                isSendedHolded = true;
            }
        }
    }

    public void UpdatedMousePosition(Vector2 pos)
    {
        CursorContainer.anchoredPosition = pos * 2;
        CursorContainer.position = mainCamera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 1f));

        CursorContainerCenter.position = CursorContainer.position;
    }

    public void RaycasterUpdated(List<RaycastResult> raycastResults)
    {
        var customButtons = raycastResults.Where(x => x.gameObject.TryGetComponent<CustomButton>(out var e)).ToList();

        if (customButtons.Count == 0)
        {
            selectedCustomButton?.CallOutHover();
            selectedCustomButton = null;
        }
        else {
            var value = customButtons.Max(x => x.depth);
            var newButton = customButtons.Where(x => x.depth == value).First().gameObject.GetComponent<CustomButton>();
            if (selectedCustomButton == newButton)
                return;
            selectedCustomButton?.CallOutHover();
            selectedCustomButton = newButton;
            newButton.CallOnHover();
        }
    }

    public void UseStarted()
    {
        isButtonHold = true;
        inputManager.CheckIsStillOnGUI();
        if (selectedCustomButton != null)
            timeToHold = selectedCustomButton.GetPressedTime();
    }

    public void UseCanceled()
    {
        if (buttonHoldTime < timeToHold)
        {
            selectedCustomButton?.CallOnClicked(mouseButton);
        }
        else
        {
            selectedCustomButton?.CallOnRelease(mouseButton);
        }
        selectedCustomButton = null;
        buttonHoldTime = 0;
        isButtonHold = false;
        isSendedHolded = false;
        inputManager.CheckIsStillOnGUI();
    }
}

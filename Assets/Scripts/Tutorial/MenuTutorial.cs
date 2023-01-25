using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameMenu;

public class MenuTutorial : MonoBehaviour
{
    public static bool IsTutorialEnded;
    public bool isTutorialFinished;
    public int stage;
    public float T;

    [SerializeField] private Transform Aim1;
    [SerializeField] private Transform Target1;
    [SerializeField] private Transform Target2;

    [SerializeField] List<Image> attractImages;
    [SerializeField] RectTransform AttractorRect;

    public static MenuTutorial singleton;

    private void Awake()
    {
        singleton = this;
        stage = -1;
    }

    public static void TutorialBroken() {
        if (singleton != null) {
            singleton.TutorialBroke();
        }
    }

    public void TutorialBroke() {
        T = 1f - attractImages[0].color.a / 0.75f;
        stage = 3;
    }

    void Start()
    {
        if (KeyManager.GetKey("Tutorial-menu", 0) == 1)
        {
            IsTutorialEnded = true;
        }

        if (!IsTutorialEnded)
        {
            if (KeyManager.GetKey("Coins") >= 1)
            {
            }
            else {
                gameObject.SetActive(false);
            }
        }
        else {
            TutorialEnded();
        }
    }

    void Update()
    {
        if (stage < 2) {
            if (GM_Manager.singleton.SelectedIdNow == 1) {
                stage = 2;
            }
        }

        switch (stage)
        {
            case -1:
                T += Time.deltaTime;
                if (T >= 0.5f)
                {
                    T = 0;
                    Aim1.position = Target1.position;
                    stage = 0;
                }
                break;
            case 0:
                T += Time.deltaTime * 2;
                var c = new Color(0, 0, 0, T * 0.75f);
                foreach (var e in attractImages) {
                    e.color = c;
                }
                if (T >= 1)
                {
                    c = new Color(0, 0, 0, 0.75f);
                    foreach (var e in attractImages)
                    {
                        e.color = c;
                    }
                    T = 0;
                    stage = 1;
                }
                break;
            case 2:
                T += Time.deltaTime;
                var value = Mathf.Lerp(400, 250, T / 1.5f);
                AttractorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                AttractorRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

                Aim1.position = Vector3.Lerp(Aim1.position, Target2.position, Time.deltaTime * 3);

                if (KeyManager.GetKey("Slot_Plant-2-1", 0) > 0 || KeyManager.GetKey("Slot_Plant-2-2", 0) > 0 || KeyManager.GetKey("Coins", 0) < 1)
                {
                    isTutorialFinished = true;
                    T = 0;
                    stage = 3;
                }
                break;
            case 3:
                T += Time.deltaTime * 2;
                var c2 = new Color(0, 0, 0, (1f - T) * 0.75f);
                foreach (var e in attractImages)
                {
                    e.color = c2;
                }
                if (T >= 1)
                {
                    T = 0;
                    c2 = new Color(0, 0, 0, 0f);
                    foreach (var e in attractImages)
                    {
                        e.color = c2;
                    }
                    stage = 4;
                    TutorialEnded();
                }
                break;
        }
    }

    void TutorialEnded()
    {
        if (isTutorialFinished)
        {
            IsTutorialEnded = true;
            KeyManager.SetKey("Tutorial-menu", 1);
        }
        Destroy(gameObject);
    }
}

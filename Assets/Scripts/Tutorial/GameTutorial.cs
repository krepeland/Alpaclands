using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial : MonoBehaviour
{
    public static bool IsTutorialEnded;
    private int stage;

    float T;

    [SerializeField] private GameObject MovingTutorial;
    [SerializeField] private GameObject RotatingTutorial;
    [SerializeField] private Transform ProgressBar;

    public static GameTutorial singleton;


    private void Awake()
    {
        singleton = this;
        transform.localScale = new Vector3(0, 1, 1);
    }

    void Start()
    {
        if (KeyManager.GetKey("Tutorial-game", 0) == 1) {
            IsTutorialEnded = true;
        }

        stage = -1;
        if (IsTutorialEnded)
            TutorialEnded(1.5f);
    }

    public void EnableTutorial() {
        gameObject.SetActive(true);
        MovingTutorial.SetActive(true);
        RotatingTutorial.SetActive(false);
    }

    void Update()
    {
        switch (stage) {
            case -1:
                T += Time.deltaTime;
                if (T >= 1.5f) {
                    T = 0;
                    stage = 0;
                }
                break;
            case 0:
                T += Time.deltaTime * 2;
                if (T >= 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    T = 0;
                    stage = 1;
                }
                else {
                    transform.localScale = new Vector3(T, 1, 1);
                }
                break;
            case 1:
                T += Time.deltaTime * 0.5f;

                if (InputManager.singleton.Move.magnitude != 0 || (InputManager.singleton.UseTime > 0))
                {
                    T += Time.deltaTime * 3;
                }

                ProgressBar.localScale = new Vector3(Mathf.Min(1, T / 10f), 1, 1);
                if (T >= 10.5f)
                {
                    T = 0;
                    stage = 2;
                    ProgressBar.localScale = new Vector3(1, 1, 1);
                }
                break;
            case 2:
                T += Time.deltaTime * 2;
                if (T >= 1)
                {
                    transform.localScale = new Vector3(0, 1, 1);
                    MovingTutorial.SetActive(false);
                    RotatingTutorial.SetActive(true);
                    if (T >= 1.25f) {
                        T = 0;
                        stage = 3;
                        ProgressBar.localScale = new Vector3(0, 1, 1);
                        ProgressBar.localScale = new Vector3(0, 1, 1);
                    }
                }
                else
                {
                    transform.localScale = new Vector3(1 - T, 1, 1);
                }
                break;
            case 3:
                T += Time.deltaTime * 2;

                if (T >= 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    T = 0;
                    stage = 4;
                }
                else
                {
                    transform.localScale = new Vector3(T, 1, 1);
                }
                break;
            case 4:
                T += Time.deltaTime * 0.5f;

                if (InputManager.singleton.Rotate != 0 || (InputManager.singleton.AlternativeUseTime > 0))
                {
                    T += Time.deltaTime * 3;
                }

                ProgressBar.localScale = new Vector3(Mathf.Min(1, T / 10f), 1, 1);

                if (T >= 10.5f)
                {
                    stage = 5;
                    T = 0;
                    ProgressBar.localScale = new Vector3(1, 1, 1);
                }
                break;
            case 5:
                T += Time.deltaTime * 2;
                if (T >= 1)
                {
                    transform.localScale = new Vector3(0, 1, 1);
                    if (T >= 1.25f)
                    {
                        TutorialEnded(0);
                    }
                }
                else
                {
                    transform.localScale = new Vector3(1 - T, 1, 1);
                }
                break;
        }
    }

    public void Skip() {
        TutorialEnded(0);
    }

    void TutorialEnded(float t)
    {
        KeyManager.SetKey("Tutorial-game", 1);
        IsTutorialEnded = true;
        CardManager.singleton.StartCards(t);
        Destroy(gameObject);
    }
}

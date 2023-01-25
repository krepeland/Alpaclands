using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndCard : CustomButton
{
    public bool isResizing;
    public float nowX;
    public float targetX;

    public static LevelEndCard singleton;

    public bool isLoading;
    public bool ended;

    [SerializeField] private ScoreToCoins scoreToCoins;

    private void Awake()
    {
        singleton = this;
        nowX = 0;
        transform.localScale = new Vector3(nowX, 1, 1);
        gameObject.SetActive(false);
        enabled = false;
    }

    public void LevelEnded()
    {
        ended = true;
        gameObject.SetActive(true);
        enabled = true;
        StartCoroutine(EndLevel());
    }

    IEnumerator EndLevel() {
        yield return new WaitForSeconds(1f);
        MoneyGetter.singleton.Show();
        scoreToCoins.gameObject.SetActive(true);
        KeyManager.SetKey("Score", ProgressBar.singleton.PointsNow);
        UiHideElement.Hide = true;
        targetX = 1;
        isResizing = true;
        yield return new WaitForSeconds(1f);
    }

    private void Update()
    {

        if (!isResizing)
            return;

        nowX = Mathf.Clamp01(Mathf.Lerp(nowX, targetX, Time.deltaTime / Mathf.Abs(nowX - targetX) * 2));
        if (nowX == targetX)
        {
            isResizing = false;
        }
        Resizing();
    }

    void Resizing()
    {
        transform.localScale = IsHovered ? new Vector3(nowX * 1.1f, 1.1f, 1) : new Vector3(nowX, 1, 1);
    }

    public override void OnHover()
    {
        Resizing();
    }

    public override void OutHover()
    {
        Resizing();
    }

    public override void OnClicked(int mouseButton)
    {
        if (isLoading)
            return;
        isLoading = true;
        isResizing = true;
        targetX = 0;
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0.5f);
        CameraController.singleton.LevelEnded = true;
        yield return new WaitForSeconds(4f);
        UiHideElement.Hide = false;
        UiHideElement2.Hide = false;
        StartGame.singleton.SaveNewData();
        SceneManager.LoadScene(1);
    }
}

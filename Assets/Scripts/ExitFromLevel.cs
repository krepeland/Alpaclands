using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromLevel : CustomButton
{
    Vector2 startSize;
    bool isClicked;

    private void Awake()
    {
        startSize = transform.localScale;
    }

    public override void OnHover()
    {
        transform.localScale = startSize * 1.1f;
    }

    public override void OutHover()
    {
        transform.localScale = startSize;
    }

    public override void OnClicked(int mouseButton)
    {
        if (isClicked)
            return;
        isClicked = true;
        StartCoroutine(InstantExitToMenu());
    }

    IEnumerator InstantExitToMenu()
    {
        KeyManager.SetKey("Score", ProgressBar.singleton.PointsNow);
        TakeableCoin.TakeAllCoins(1.5f);
        MoneyGetter.singleton.Show();
        UiHideElement.Hide = true;
        UiHideElement2.Hide = true;
        CardPack.singleton.Exited();
        CardManager.singleton.RemoveCardFromHandExited();
        yield return new WaitForSeconds(0.25f);

        if (!LevelEndCard.singleton.ended)
        {
            for (var i = 0; i < ProgressBar.singleton.PointsNow / 100; i++)
            {
                MoneyGetter.singleton.GetCoin();
                yield return new WaitForSeconds(0.1f);
            }
        }

        CameraController.singleton.LevelEnded = true;
        yield return new WaitForSeconds(4f);
        UiHideElement.Hide = false;
        UiHideElement2.Hide = false;

        StartGame.singleton.SaveNewData();
        SceneManager.LoadScene(1);
    }
}

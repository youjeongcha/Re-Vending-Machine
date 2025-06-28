using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class InGameManager : MonoBehaviour
{
    public int Score { get; private set; }

    public float countdownTime = 5.0f;
    private Coroutine countdownRoutine;

    void Start()
    {
        GameManager.Instance.SetState(GameState.Playing);
        PlayGameUIMgr.Instance.ShowHUD();

        // 배너 광고
        AdManager.Instance.LoadBannerAd(); // 새 씬에 들어올 때 반드시 다시 호출!
    }

    public void AdDoubleCoin()
    {
        AdManager.Instance.ShowRewardAd(() => {
            CoinManager.Instance.AddCoins(Score / 5); // 2배 지급
        });
    }


    public void AddScore(int value)
    {
        Score += value;
        PlayGameUIMgr.Instance.UpdateScore(Score);
    }

    public void GameOver()
    {
        GameManager.Instance.SetState(GameState.GameOver);
        PlayGameUIMgr.Instance.ShowGameOver();
        CoinManager.Instance.AddCoins(Score / 10); // 점수의 10%를 코인으로
    }

    public void Revive()
    {
        PlayGameUIMgr.Instance.HideGameOverUI();

        //TODO::광고 점검 필요
        AdManager.Instance.ShowRewardAd(() => {
            Revive();
        });
    }

    public void Restart()
    {
        Debug.Log("다시 시작");
        GameManager.Instance.SetState(GameState.Playing);
        //AdManager.Instance.ShowInterstitialAd(); //이거 광고가 자동으로 꺼져서 그런지 뭔가 다 이상해짐
        // spawner 작동 이상하고, 머지도 안되는 상황이 되어버림.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        // TODO::CoinMgr이나 스코어 데이터 저장 하는 것 필요
        GameManager.Instance.EndScene();
    }

    public void StartGameOverCountdown()
    {
        if (countdownRoutine == null)
            countdownRoutine = StartCoroutine(GameOverCountdown());
    }

    public void CancelGameOverCountdown()
    {
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
            PlayGameUIMgr.Instance.HideGameOverCountdown(); // 선택 사항
        }
    }

    private IEnumerator GameOverCountdown()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            PlayGameUIMgr.Instance.UpdateGameOverCountdown(timer); // UI에 남은 시간 표시
            timer -= Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

/*    public void UploadScoreAndSave()
    {
        SaveManager.SaveScore(Score);
        RankManager.TryUpload(Score);
    }*/
}

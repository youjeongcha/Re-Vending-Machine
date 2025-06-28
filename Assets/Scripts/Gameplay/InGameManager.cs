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

        // ��� ����
        AdManager.Instance.LoadBannerAd(); // �� ���� ���� �� �ݵ�� �ٽ� ȣ��!
    }

    public void AdDoubleCoin()
    {
        AdManager.Instance.ShowRewardAd(() => {
            CoinManager.Instance.AddCoins(Score / 5); // 2�� ����
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
        CoinManager.Instance.AddCoins(Score / 10); // ������ 10%�� ��������
    }

    public void Revive()
    {
        PlayGameUIMgr.Instance.HideGameOverUI();

        //TODO::���� ���� �ʿ�
        AdManager.Instance.ShowRewardAd(() => {
            Revive();
        });
    }

    public void Restart()
    {
        Debug.Log("�ٽ� ����");
        GameManager.Instance.SetState(GameState.Playing);
        //AdManager.Instance.ShowInterstitialAd(); //�̰� ���� �ڵ����� ������ �׷��� ���� �� �̻�����
        // spawner �۵� �̻��ϰ�, ������ �ȵǴ� ��Ȳ�� �Ǿ����.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        // TODO::CoinMgr�̳� ���ھ� ������ ���� �ϴ� �� �ʿ�
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
            PlayGameUIMgr.Instance.HideGameOverCountdown(); // ���� ����
        }
    }

    private IEnumerator GameOverCountdown()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            PlayGameUIMgr.Instance.UpdateGameOverCountdown(timer); // UI�� ���� �ð� ǥ��
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

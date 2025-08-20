using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayGameUIMgr : MonoBehaviour
{
    public static PlayGameUIMgr Instance;

    public InGameManager InGameMgr;

    public GameObject hudPanel; //HUD (scoreText, coinText)
    public GameObject gameOverPanel;
    

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI coinText;

    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI nowScoreText;

    public Button doubleRewardCoin;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void ShowHUD()
    {
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void ShowBestScore(int score)
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best Score: {score}";
        }
    }

    public void ShowNowScore(int score)
    {
        if (nowScoreText != null)
        {
            nowScoreText.text = $"Now Score: {score}";
        }
    }

    public void HideGameOverCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }
    public void UpdateGameOverCountdown(float time)
    {
        countdownText.text = $"{time:F1}s countdown!";
        countdownText.gameObject.SetActive(true);
    }

    public void ShowGameOver(int score)
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        ShowBestScore(SaveManager.Instance.BestScore);
        ShowNowScore(score);
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = $"Coin: {coin}";
    }

    public void DoubleRewardCoin(int score)
    {
        AdManager.Instance.ShowRewardAd(() => {
            Debug.Log($"광고 2배 획득: {score} {score / 5}");

            CoinManager.Instance.AddCoins(score / 5); // 2배 지급
        });

        doubleRewardCoin.gameObject.SetActive(false);
    }

    public void HideGameOverUI()
    {
        gameOverPanel.SetActive(false);
        //revivePanel.SetActive(false);
    }

/*    public void OnClickReviveByAd()
    {
        AdManager.Instance.ShowRewardAd(() => {
            InGameMgr.Revive();
        });
    }

    public void OnClickReviveByCoin()
    {
        if (CoinManager.Instance.Coin >= 100) // 예: 100코인 부활
        {
            CoinManager.Instance.UseCoins(100);
            InGameMgr.Revive();
        }
        else
        {
            // 코인 부족 알림
        }
    }*/

    public void ShowGachaResult(CharacterData data)
    {
        // Display result popup with character info
    }
}
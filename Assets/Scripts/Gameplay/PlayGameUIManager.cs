using TMPro;
using UnityEngine;
using TMPro;

public class PlayGameUIMgr : MonoBehaviour
{
    public static PlayGameUIMgr Instance;

    public InGameManager InGameMgr;

    public GameObject hudPanel; //HUD (scoreText, coinText)
    public GameObject gameOverPanel;
    

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI coinText;

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

    public void HideGameOverCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }
    public void UpdateGameOverCountdown(float time)
    {
        countdownText.text = $"{time:F1}s countdown!";
        countdownText.gameObject.SetActive(true);
    }

    public void ShowGameOver()
    {
        hudPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = $"Coin: {coin}";
    }

    public void HideGameOverUI()
    {
        gameOverPanel.SetActive(false);
        //revivePanel.SetActive(false);
    }

    public void OnClickReviveByAd()
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
    }

    public void ShowGachaResult(CharacterData data)
    {
        // Display result popup with character info
    }
}
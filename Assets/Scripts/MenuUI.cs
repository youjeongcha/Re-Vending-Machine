using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    [Header("Button References")]
    public Button startButton;
    public Button gachaButton;
    public Button collectionButton;

    [Header("Target Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string gachaSceneName = "GachaScene";
    [SerializeField] private string collectionSceneName = "CollectionScene";

    void Start()
    {
        // 배너 광고
        AdManager.Instance.LoadBannerAd(); // 새 씬에 들어올 때 반드시 다시 호출!

        if (startButton != null)
            startButton.onClick.AddListener(() => LoadScene(gameSceneName));

        if (gachaButton != null)
            gachaButton.onClick.AddListener(() => LoadScene(gachaSceneName));

        if (collectionButton != null)
            collectionButton.onClick.AddListener(() => LoadScene(collectionSceneName));
    }

    public void ShowCoin(int coin)
    {
        if (coinText == null) 
            Debug.LogWarning("coinText.Instance is null");


        coinText.text = $"Coin: {coin}";
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
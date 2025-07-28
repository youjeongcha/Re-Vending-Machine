using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] MenuUI menuUI;

    public int Coin { get; private set; }


    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        Coin = SaveManager.Instance.LoadCoin(); // 안전한 방식

/*        if (menuUI == null) Debug.LogWarning("MenuUI.Instance is null");
        if (SaveManager.Instance == null) Debug.LogWarning("SaveManager.Instance is null");
        Coin = SaveManager.Instance?.LoadCoin() ?? 0;

        Debug.Log("Coin : " + Coin);*/


        menuUI.ShowCoin(Coin);
    }

    public void AddCoins(int amount)
    {
        Coin += amount;
        SaveManager.Instance.SaveCoin(Coin);
        PlayGameUIMgr.Instance.UpdateCoin(Coin);
    }

    public bool UseCoins(int amount)
    {
        if (Coin >= amount)
        {
            Coin -= amount;
            SaveManager.Instance.SaveCoin(Coin);
            PlayGameUIMgr.Instance.UpdateCoin(Coin);
            return true;
        }
        return false;
    }
}
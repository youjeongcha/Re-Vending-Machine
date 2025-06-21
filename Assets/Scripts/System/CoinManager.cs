using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int Coin { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        Coin = PlayerPrefs.GetInt("Coin", 0);
    }

    public void AddCoins(int amount)
    {
        Coin += amount;
        Save();
    }

    public bool UseCoins(int amount)
    {
        if (Coin >= amount)
        {
            Coin -= amount;
            Save();
            return true;
        }
        return false;
    }

    void Save()
    {
        PlayerPrefs.SetInt("Coin", Coin);
    }
}
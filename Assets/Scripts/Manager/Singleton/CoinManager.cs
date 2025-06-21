using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int Coin { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 안 사라지게
        }
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

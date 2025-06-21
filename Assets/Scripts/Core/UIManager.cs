using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text scoreText;
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowGachaResult(CharacterData data)
    {
        // Display result popup with character info
    }
}
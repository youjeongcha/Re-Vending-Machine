using UnityEngine;
using UnityEngine.Playables;

public enum GameState
{
    None,
    Loading,
    Playing,
    GameOver,
    Gacha,
    Pause,
    PvP,
    Event
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState { get; private set; } = GameState.None;

    public bool IsGameOver { get; private set; }
    public int Score { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        Score += value;
        UIManager.Instance.UpdateScore(Score);
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
        UIManager.Instance.ShowGameOver();
        CoinManager.Instance.AddCoins(Score / 10);
    }

    public void SetState(GameState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"GameState ����: {currentState} �� {newState}");
        currentState = newState;

        switch (newState)
        {
            case GameState.Loading:
                // �ʱ�ȭ ����
                break;
            case GameState.Playing:
                // �÷��� ���� ����
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOver();
                CoinManager.Instance.AddCoins(GameData.Instance.Score / 10);
                break;
            case GameState.Gacha:
                // Gacha �� �̵�
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            case GameState.Event:
                // �̺�Ʈ ó��
                break;
        }
    }

    public void GameOver()
    {

    }
}
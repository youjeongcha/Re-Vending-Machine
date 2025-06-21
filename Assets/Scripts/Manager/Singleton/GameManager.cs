using System.Collections;
using System.Collections.Generic;
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
    public GameState CurrentState { get; private set; } = GameState.None;

    public bool IsGameOver { get; private set; }
    public int Score { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 안 사라지게
        }
        else Destroy(gameObject); // 중복된 건 제거
    }

    public void AddScore(int value)
    {
        Score += value;
        UIManager.Instance.UpdateScore(Score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        UIManager.Instance.ShowGameOver();
        CoinManager.Instance.AddCoins(Score / 10);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        Debug.Log($"GameState 변경: {CurrentState} → {newState}");
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Loading:
                // 초기화 로직
                break;
            case GameState.Playing:
                // 플레이 시작 로직
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOver();
                CoinManager.Instance.AddCoins(GameData.Instance.Score / 10);
                break;
            case GameState.Gacha:
                // Gacha 씬 이동
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            case GameState.Event:
                // 이벤트 처리
                break;
        }
    }
}
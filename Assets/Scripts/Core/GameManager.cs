using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum GameState
{
    Main,
    Menu,
   // Loading,
    Playing,
    GameOver,
    Pause,
    Gacha,
    PvP,
    Event
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState { get; private set; } = GameState.Main;

    /*    public bool IsGameOver { get; private set; }
        public int Score { get; private set; }*/


    /*    public float countdownTime = 5.0f;
        private Coroutine countdownRoutine;*/

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveManager.Instance.LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetState(GameState newState)
    {
        //AdManager.Instance.ShowBanner();
        currentState = newState;
        Debug.Log($"GameState ¡æ {newState}");
    }

    public void EndScene()
    {
        currentState = GameState.Main;
        SceneManager.LoadScene("MenuScene");
    }




}
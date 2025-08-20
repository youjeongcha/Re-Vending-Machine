using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Threading.Tasks;
using System;


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

    public NicknameUI nicknameUI;

    public GameObject touchScreen;
    public GameObject nickNamePannel;


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


    async void Start()
    {
        try
        {
            while (!FirebaseManager.Instance.IsInitialized)
                await Task.Delay(100); // Firebase가 초기화될 때까지 대기

            // 닉네임이 저장되어 있는지 확인하고, 없으면 창 열기
            if (string.IsNullOrEmpty(FirebaseManager.Instance.Nickname))
            {
                Debug.Log($"닉네임 없는걸로 뜸");

                touchScreen.SetActive(false); // 닉네임 설정 중엔 터치 스크린 비활성화
                nicknameUI.OnNicknameConfirmed = OnNicknameConfirmed; // 콜백함수
                nicknameUI.Open();
            }
            else
            {
                // 닉네임 설정 패널 안 뜨게
                nickNamePannel.SetActive(false);
                // 이미 닉네임이 있는 경우 바로 터치스크린 활성화
                touchScreen.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Start에서 오류 발생: " + e.Message);
        }
    }

    private void OnNicknameConfirmed(string nickname)
    {
        Debug.Log($"닉네임 설정 완료: {nickname}");
        // 랭킹 데이터 저장 가능 or 게임 시작 등
        touchScreen.SetActive(true); // 닉네임 설정 완료 후 터치 스크린 활성화
    }

    public void SetState(GameState newState)
    {
        //AdManager.Instance.ShowBanner();
        currentState = newState;
        Debug.Log($"GameState → {newState}");
    }

    public void EndScene()
    {
        currentState = GameState.Main;
        SceneManager.LoadScene("MenuScene");
    }
}
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
                await Task.Delay(100); // Firebase�� �ʱ�ȭ�� ������ ���

            // �г����� ����Ǿ� �ִ��� Ȯ���ϰ�, ������ â ����
            if (string.IsNullOrEmpty(FirebaseManager.Instance.Nickname))
            {
                Debug.Log($"�г��� ���°ɷ� ��");

                touchScreen.SetActive(false); // �г��� ���� �߿� ��ġ ��ũ�� ��Ȱ��ȭ
                nicknameUI.OnNicknameConfirmed = OnNicknameConfirmed; // �ݹ��Լ�
                nicknameUI.Open();
            }
            else
            {
                // �г��� ���� �г� �� �߰�
                nickNamePannel.SetActive(false);
                // �̹� �г����� �ִ� ��� �ٷ� ��ġ��ũ�� Ȱ��ȭ
                touchScreen.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Start���� ���� �߻�: " + e.Message);
        }
    }

    private void OnNicknameConfirmed(string nickname)
    {
        Debug.Log($"�г��� ���� �Ϸ�: {nickname}");
        // ��ŷ ������ ���� ���� or ���� ���� ��
        touchScreen.SetActive(true); // �г��� ���� �Ϸ� �� ��ġ ��ũ�� Ȱ��ȭ
    }

    public void SetState(GameState newState)
    {
        //AdManager.Instance.ShowBanner();
        currentState = newState;
        Debug.Log($"GameState �� {newState}");
    }

    public void EndScene()
    {
        currentState = GameState.Main;
        SceneManager.LoadScene("MenuScene");
    }
}
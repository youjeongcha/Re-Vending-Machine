using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToChangeScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "MenuScene";
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
/*
    public void GotoNextScene() // 직접호출용
    {
        SceneManager.LoadScene(nextSceneName);
    }*/
}

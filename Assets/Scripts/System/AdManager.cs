using System;
using System.Collections;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowRewardAd(Action onSuccess)
    {
        // ���� ���� ������ Unity Ads ���� �ʿ�
        // �������� �ٷ� ���� ó��
        Debug.Log("���� ��� ��...");
        StartCoroutine(FakeAdRoutine(onSuccess));
    }

    private IEnumerator FakeAdRoutine(Action onSuccess)
    {
        yield return new WaitForSeconds(2f); // ���� ���� �ð� �䳻 ���� �ʿ�
        onSuccess?.Invoke();
    }

    public void ShowInterstitialAd() { }

    public void ShowBanner() { }
}
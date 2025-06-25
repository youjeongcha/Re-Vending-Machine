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
        // 실제 광고 로직은 Unity Ads 연동 필요
        // 예제에선 바로 성공 처리
        Debug.Log("광고 재생 중...");
        StartCoroutine(FakeAdRoutine(onSuccess));
    }

    private IEnumerator FakeAdRoutine(Action onSuccess)
    {
        yield return new WaitForSeconds(2f); // 광고 보는 시간 흉내 삭제 필요
        onSuccess?.Invoke();
    }

    public void ShowInterstitialAd() { }

    public void ShowBanner() { }
}
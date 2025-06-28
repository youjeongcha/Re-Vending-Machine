using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;
    private BannerView bannerView;

    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MobileAds.Initialize(initStatus => { });

            // 광고 로드
            LoadRewardedAd();
            LoadInterstitialAd();
            LoadBannerAd();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsRewardAdReady()
    {
        return rewardedAd != null && rewardedAd.CanShowAd();
    }



    // 보상형 광고
    public void LoadRewardedAd()
    {
        var adRequest = new AdRequest();

        RewardedAd.Load(
            rewardedAdUnitId,
            adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError($"[AdManager] 보상형 광고 로드 실패: {error?.GetMessage()}");
                    return;
                }

                rewardedAd = ad;

                rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("[AdManager] 광고 닫힘");
                    LoadRewardedAd(); // 광고 닫히면 재로딩
                };

                rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
                {
                    Debug.LogError($"[AdManager] 광고 표시 실패: {adError.GetMessage()}");
                };

                Debug.Log("[AdManager] 보상형 광고 로드 완료");
            }
        );
    }

    public void ShowRewardAd(Action onSuccess)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"광고 보상 획득: {reward.Amount} {reward.Type}");
                onSuccess?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("보상형 광고를 재생할 수 없습니다.");
            onSuccess?.Invoke();  // 테스트 중엔 그냥 통과
        }
    }



    // 전면 광고 interstitialAd
    public void LoadInterstitialAd()
    {
        var adRequest = new AdRequest();

        InterstitialAd.Load(
            interstitialAdUnitId,
            adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError($"[AdManager] 전면 광고 로드 실패: {error?.GetMessage()}");
                    return;
                }

                interstitialAd = ad;

                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("[AdManager] 전면 광고 닫힘");
                    LoadInterstitialAd(); // 닫힌 후 재로드
                };

                interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
                {
                    Debug.LogError($"[AdManager] 전면 광고 표시 실패: {adError.GetMessage()}");
                };

                Debug.Log("[AdManager] 전면 광고 로드 완료");
            }
        );
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("[AdManager] 전면 광고가 준비되지 않음");
        }
    }


    // 배너 광고 - 매 씬 넘어가면 사라져서 씬마다 로드 필요(유니티가 관리하지 않음)
    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy(); // 기존 배너 정리
        }

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        var adRequest = new AdRequest();

        bannerView.LoadAd(adRequest);
        //bannerView.Show(); // <-- Load 직후 바로 Show 가능
    }


/*    public void ShowBanner()
    {
        Debug.Log("들어오긴 하냐?");
        if (bannerView == null)
        {
            Debug.Log("null 테스트");
        }
        bannerView?.Show();
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }*/

}
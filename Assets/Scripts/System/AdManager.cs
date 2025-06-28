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

            // ���� �ε�
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



    // ������ ����
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
                    Debug.LogError($"[AdManager] ������ ���� �ε� ����: {error?.GetMessage()}");
                    return;
                }

                rewardedAd = ad;

                rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("[AdManager] ���� ����");
                    LoadRewardedAd(); // ���� ������ ��ε�
                };

                rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
                {
                    Debug.LogError($"[AdManager] ���� ǥ�� ����: {adError.GetMessage()}");
                };

                Debug.Log("[AdManager] ������ ���� �ε� �Ϸ�");
            }
        );
    }

    public void ShowRewardAd(Action onSuccess)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"���� ���� ȹ��: {reward.Amount} {reward.Type}");
                onSuccess?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("������ ���� ����� �� �����ϴ�.");
            onSuccess?.Invoke();  // �׽�Ʈ �߿� �׳� ���
        }
    }



    // ���� ���� interstitialAd
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
                    Debug.LogError($"[AdManager] ���� ���� �ε� ����: {error?.GetMessage()}");
                    return;
                }

                interstitialAd = ad;

                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("[AdManager] ���� ���� ����");
                    LoadInterstitialAd(); // ���� �� ��ε�
                };

                interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
                {
                    Debug.LogError($"[AdManager] ���� ���� ǥ�� ����: {adError.GetMessage()}");
                };

                Debug.Log("[AdManager] ���� ���� �ε� �Ϸ�");
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
            Debug.LogWarning("[AdManager] ���� ���� �غ���� ����");
        }
    }


    // ��� ���� - �� �� �Ѿ�� ������� ������ �ε� �ʿ�(����Ƽ�� �������� ����)
    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy(); // ���� ��� ����
        }

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        var adRequest = new AdRequest();

        bannerView.LoadAd(adRequest);
        //bannerView.Show(); // <-- Load ���� �ٷ� Show ����
    }


/*    public void ShowBanner()
    {
        Debug.Log("������ �ϳ�?");
        if (bannerView == null)
        {
            Debug.Log("null �׽�Ʈ");
        }
        bannerView?.Show();
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }*/

}
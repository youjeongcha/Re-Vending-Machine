using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 안 사라지게
        }
        else Destroy(gameObject);
    }

    public void ShowRewardAd(Action onSuccess)
    {
        // Dummy structure  - link with Unity Ads / AdMob reward ad callback
        bool watched = true; // simulate
        if (watched) onSuccess?.Invoke();
    }

    public void ShowInterstitialAd()
    {
        // Show interstitial
    }

    public void ShowBanner()
    {
        // Show banner ad
    }
}
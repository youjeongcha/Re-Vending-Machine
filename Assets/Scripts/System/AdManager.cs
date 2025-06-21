using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public void ShowRewardAd(Action onSuccess)
    {
        bool watched = true;
        if (watched) onSuccess?.Invoke();
    }

    public void ShowInterstitialAd() { }

    public void ShowBanner() { }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// SoundManager.cs 확장: 병합 이펙트마다 다른 효과음 + 씬마다 다른 BGM 지원

/*
다음 기능을 모두 포함:

배경음(BGM)과 효과음(SFX) 구분 재생
AudioMixer를 통한 마스터 볼륨 조절
랜덤 병합 효과음, 공 드랍 효과음 재생
사운드 뮤트 토글 기능 (ToggleMute)
UI와 슬라이더로 연결 시 실시간 조절 가능*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Mixer & Audio Sources")]
    public AudioMixer audioMixer; // Master Mixer //전체 볼륨, BGM 볼륨, SFX 볼륨을 조절하는 AudioMixer 자원
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM by Scene")]
    public BGMEntry[] bgmEntries;

    [Header("병합 효과음 by 이펙트 이름")]
    public MergeSFXEntry[] mergeSFXEntries; // 여러 개를 넣어두고 PlayRandomMergeSFX()에서 랜덤으로 선택해서 재생
    // 병합 시 SoundManager.Instance.PlayMergeSFXByEffect("SmokePop")처럼 호출

    public AudioClip dropSfxClip;
    private bool isMuted = false;


    [System.Serializable]
    public class BGMEntry
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [System.Serializable]
    public class MergeSFXEntry
    {
        public string effectName;
        public AudioClip sfxClip;
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var entry in bgmEntries)
        {
            if (entry.sceneName == scene.name)
            {
                PlayBGM(entry.bgmClip);
                return;
            }
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMergeSFXByEffect(string effectName)
    {
        foreach (var entry in mergeSFXEntries)
        {
            if (entry.effectName == effectName)
            {
                PlaySFX(entry.sfxClip);
                return;
            }
        }
    }

    public void PlayDropSFX()
    {
        PlaySFX(dropSfxClip);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20); // 0.0001~1.0 → -80~0dB
    }

    public void SetBGMVolume(float volume) // 0~1 → -80~0 dB
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Lerp(-80f, 0f, volume));
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80f, 0f, volume));
    }


    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioMixer.SetFloat("MasterVolume", isMuted ? -80f : 0f);
    }
}

// Audio Mixer 설정 가이드는 아래와 같음:
// - Master 믹서 그룹 만들기 (Master)
//     - BGM 그룹 (BGM)
//     - SFX 그룹 (SFX)
// - Master → exposed parameter: MasterVolume (-80 ~ 0 dB)
// - BGM, SFX → 각각 그룹에 AudioSource 할당

// Unity Editor 설정:
// - AudioMixer 만들기 (ex: MainMixer)
// - BGM용 AudioSource에는 Output을 BGM 그룹으로, SFX는 SFX 그룹으로
// - SoundManager에 AudioMixer, Source, Clip들 연결
// - UI 토글 및 슬라이더로 볼륨/뮤트 조절 연결 가능


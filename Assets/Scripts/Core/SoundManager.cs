using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// SoundManager.cs Ȯ��: ���� ����Ʈ���� �ٸ� ȿ���� + ������ �ٸ� BGM ����

/*
���� ����� ��� ����:

�����(BGM)�� ȿ����(SFX) ���� ���
AudioMixer�� ���� ������ ���� ����
���� ���� ȿ����, �� ��� ȿ���� ���
���� ��Ʈ ��� ��� (ToggleMute)
UI�� �����̴��� ���� �� �ǽð� ���� ����*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Mixer & Audio Sources")]
    public AudioMixer audioMixer; // Master Mixer //��ü ����, BGM ����, SFX ������ �����ϴ� AudioMixer �ڿ�
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM by Scene")]
    public BGMEntry[] bgmEntries;

    [Header("���� ȿ���� by ����Ʈ �̸�")]
    public MergeSFXEntry[] mergeSFXEntries; // ���� ���� �־�ΰ� PlayRandomMergeSFX()���� �������� �����ؼ� ���
    // ���� �� SoundManager.Instance.PlayMergeSFXByEffect("SmokePop")ó�� ȣ��

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
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20); // 0.0001~1.0 �� -80~0dB
    }

    public void SetBGMVolume(float volume) // 0~1 �� -80~0 dB
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

// Audio Mixer ���� ���̵�� �Ʒ��� ����:
// - Master �ͼ� �׷� ����� (Master)
//     - BGM �׷� (BGM)
//     - SFX �׷� (SFX)
// - Master �� exposed parameter: MasterVolume (-80 ~ 0 dB)
// - BGM, SFX �� ���� �׷쿡 AudioSource �Ҵ�

// Unity Editor ����:
// - AudioMixer ����� (ex: MainMixer)
// - BGM�� AudioSource���� Output�� BGM �׷�����, SFX�� SFX �׷�����
// - SoundManager�� AudioMixer, Source, Clip�� ����
// - UI ��� �� �����̴��� ����/��Ʈ ���� ���� ����


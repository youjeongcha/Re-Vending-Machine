using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType { BGM, SFX }
    public VolumeType volumeType;

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if (volumeType == VolumeType.BGM)
            SoundManager.Instance.SetBGMVolume(value);
        else
            SoundManager.Instance.SetSFXVolume(value);
    }
}

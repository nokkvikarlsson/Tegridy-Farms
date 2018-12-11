using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public AudioMixer audioMixer;
    public Slider musicVolumeSlider;

    public AudioMixer effectAudioMixer;
    public Slider effectVolumeSlider;

    public void Start ()
    {
        PlayerPrefs.DeleteKey("MusicVolume");
        //musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
    }

    public void SetVolume(float sliderValue)
	{
		audioMixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        //PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetEffectVolume (float effectSliderValue)
    {
        effectAudioMixer.SetFloat("effectVolume", Mathf.Log10(effectSliderValue) * 20);
    }

}

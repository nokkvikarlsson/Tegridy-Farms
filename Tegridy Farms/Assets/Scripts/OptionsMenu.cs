using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public AudioMixer audioMixer;
    public Slider musicVolumeSlider;

    public void Start ()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
    }

    public void SetVolume(float sliderValue)
	{
		audioMixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}

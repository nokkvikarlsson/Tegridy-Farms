using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundController : MonoBehaviour {

    public AudioMixerGroup audioMixer;
    public Sound[] effectSounds;
    public Sound[] farmerSounds;
    public Sound[] laundererSounds;
    public Sound[] policeSounds;
    public Sound[] landLordSounds;
    public Sound[] harvestingSounds;
    public Sound[] harvestingMethSounds;
    public Sound[] plantingSounds;

    void Awake()
    {
        foreach (Sound s in effectSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in farmerSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in laundererSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in policeSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in landLordSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in harvestingSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in harvestingMethSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }

        foreach (Sound s in plantingSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer;
        }
    }

    public void Play(string name, Sound[] sounds)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //Writes an error message if the sound was not found
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist");
            return;
        }

        s.source.Play();
    }

    //Finds a random sound from the sound array to play.
    public void PlayRandom(Sound[] sounds)
    {

        int index = UnityEngine.Random.Range(0, sounds.Length);

        Sound s = sounds[index];

        //Writes an error message if the sound was not found
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist");
            return;
        }

        s.source.Play();
    }

}

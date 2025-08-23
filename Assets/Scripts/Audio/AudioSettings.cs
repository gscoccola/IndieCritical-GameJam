using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : Singleton<AudioSettings>
{    
    public AudioMixerGroup masterMixerGroup, musicMixerGroup, sfxMixerGroup;
    public AudioMixer masterMixer;

    //IMPORTANT: Any sliders hooked up to these methods must have a minimum value of 0.0001 and a max of 1
    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

}

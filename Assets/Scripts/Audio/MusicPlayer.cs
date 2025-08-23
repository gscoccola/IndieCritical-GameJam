using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : Singleton<MusicPlayer>
{
    public AudioClip music, ambience, opera, orobourusAmbience, blender;
    public float musicVolume, ambienceVolume, operaVolume, orobourusAmbienceVolume, blenderVolume;
    private AudioSource musicSource, ambienceSource, operaSource, orobourusAmbienceSource, blenderSource;
    private bool shouldPlayOpera = false /*, triggeredBlender = false*/;

    public RandomSoundContainer significantAction;


    private void Start()
    {

        musicSource = this.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.pitch = 1;
        musicSource.spatialBlend = 0;
        musicSource.outputAudioMixerGroup = AudioSettings.Instance.musicMixerGroup;

        ambienceSource = this.AddComponent<AudioSource>();
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.pitch = 1;
        ambienceSource.spatialBlend = 0;
        ambienceSource.Play();
        ambienceSource.outputAudioMixerGroup = AudioSettings.Instance.sfxMixerGroup;


        operaSource = this.AddComponent<AudioSource>();
        operaSource.clip = opera;
        operaSource.loop = true;
        operaSource.volume = 0;
        operaSource.pitch = 1;
        operaSource.spatialBlend = 0;
        operaSource.Play();
        operaSource.outputAudioMixerGroup = AudioSettings.Instance.musicMixerGroup;


    }

    public void StopBlender()
    {
        StartCoroutine(FadeInAudioSource(musicSource, musicVolume, 1f));
        StartCoroutine(FadeInAudioSource(ambienceSource, ambienceVolume, 1));
        StartCoroutine(FadeOutAudioSource(blenderSource, blenderVolume, 0.25f));
    }

    public void StartMusic()
    {
        if (!musicSource.isPlaying) musicSource.Play();
        RandomSoundPlayer centipedeIdleSounds = GameObject.Find("Centipede").GetComponent<RandomSoundPlayer>();
        centipedeIdleSounds.StartSounds();
    }

    public void ToggleMusic (bool value)
    {

        if (value == shouldPlayOpera) return;

        if (value)
        {
            FadeToOpera();
            shouldPlayOpera = true;
        }
        else
        {
            FadeToMusic();
            shouldPlayOpera = false;
        }

    }

    private void FadeToMusic()
    {
        StartCoroutine(IFadeInMusic());
    }

    private void FadeToOpera()
    {
        StartCoroutine(IFadeInOpera());
    }

    private IEnumerator IFadeInMusic()
    {

        if (musicSource == null || operaSource == null) yield break;

        StartCoroutine(FadeInAudioSource(musicSource, musicVolume, 0.75f));
        StartCoroutine(FadeOutAudioSource(operaSource, operaVolume, 0.5f));

        StartCoroutine(LerpAudioSourcePitch(operaSource, 1, 0.5f, 3f, false));

    }

    private IEnumerator IFadeInOpera()
    {
        
        if(musicSource == null || operaSource == null) yield break;

        StartCoroutine(FadeInAudioSource(operaSource, operaVolume, 1.5f));
        StartCoroutine(FadeOutAudioSource(musicSource, musicVolume, 1));

        StartCoroutine(LerpAudioSourcePitch(operaSource, 0.5f, 1, 3f, true));

    }

    private IEnumerator LerpAudioSourcePitch(AudioSource source, float a, float b, float duration, bool up)
    {
        if (!source.isPlaying || source == null) yield break;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            if (source == null) break;
            float lerpProgression = timeElapsed / duration;
            lerpProgression = up ? lerpProgression * lerpProgression * lerpProgression : 1 - Mathf.Pow(1 - lerpProgression , 3);
            float newPitch = Mathf.Lerp(a, b, lerpProgression);
            source.pitch = newPitch;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator FadeOutAudioSource(AudioSource sourceToFadeOut, float targetFadeOutVolume, float duration)
    {

        //print("Trying to fade out source ID" +  sourceToFadeOut.GetInstanceID());

        if (!sourceToFadeOut.isPlaying || sourceToFadeOut == null || sourceToFadeOut.volume == 0) yield break;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            if (sourceToFadeOut == null) break;

            float fadeOutVolume = Mathf.Lerp(targetFadeOutVolume, 0, timeElapsed / duration);
            sourceToFadeOut.volume = fadeOutVolume;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator FadeInAudioSource(AudioSource sourceToFadeIn, float targetFadeInVolume, float duration)
    {
        //print("Trying to fade in source ID" +  sourceToFadeIn.GetInstanceID());
        float timeElapsed = 0;
        sourceToFadeIn.volume = 0;

        if (!sourceToFadeIn.isPlaying)
        {
            sourceToFadeIn.Play();
        }

        while (timeElapsed < duration)
        {
            if (sourceToFadeIn == null) break;
            //fade in
            float fadeInVolume = Mathf.Lerp(0, targetFadeInVolume, timeElapsed / duration);
            sourceToFadeIn.volume = fadeInVolume;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CrossfadeAudioSources(AudioSource sourceToFadeIn, float targetFadeInVolume, AudioSource sourceToFadeOut, float targetFadeOutVolume, float duration)
    {

        if (sourceToFadeIn == null || sourceToFadeOut == null) yield break;

        float timeElapsed = 0;
        if (!sourceToFadeIn.isPlaying)
        {
            sourceToFadeIn.volume = 0;
            sourceToFadeIn.Play();
        }

        while (timeElapsed < duration)
        {
            if (sourceToFadeIn == null || sourceToFadeOut == null) break;
            //fade in
            float fadeInVolume = Mathf.Lerp(0, targetFadeInVolume, timeElapsed / duration);
            sourceToFadeIn.volume = fadeInVolume;

            if (sourceToFadeOut.isPlaying)
            {
                float fadeOutVolume = Mathf.Lerp(targetFadeOutVolume, 0, timeElapsed / duration);
                sourceToFadeOut.volume = fadeOutVolume;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void SetOrobourus()
    {

        significantAction.PlayRandomSoundOnTarget(transform);

        StartCoroutine(FadeOutAudioSource(musicSource, musicVolume, 1));
        StartCoroutine(FadeOutAudioSource(ambienceSource, ambienceVolume, 1));

        orobourusAmbienceSource = this.AddComponent<AudioSource>();
        orobourusAmbienceSource.clip = orobourusAmbience;
        orobourusAmbienceSource.loop = true;
        orobourusAmbienceSource.volume = 0;
        orobourusAmbienceSource.pitch = 1;
        orobourusAmbienceSource.spatialBlend = 0;
        orobourusAmbienceSource.Play();
        orobourusAmbienceSource.outputAudioMixerGroup = AudioSettings.Instance.sfxMixerGroup;

        StartCoroutine(FadeInAudioSource(orobourusAmbienceSource, orobourusAmbienceVolume, 1));
    }

    public void SetBlender()
    {
     

        significantAction.PlayRandomSoundOnTarget(transform);

        StartCoroutine(FadeOutAudioSource(musicSource, musicVolume, 0.25f));
        StartCoroutine(FadeOutAudioSource(ambienceSource, ambienceVolume, 1));

        if (blenderSource == null) blenderSource = this.AddComponent<AudioSource>();
        blenderSource.clip = blender;
        blenderSource.loop = true;
        blenderSource.volume = 0;
        blenderSource.pitch = 1;
        blenderSource.spatialBlend = 0;
        blenderSource.Play();
        blenderSource.outputAudioMixerGroup = AudioSettings.Instance.sfxMixerGroup;

        StartCoroutine(FadeInAudioSource(blenderSource, blenderVolume, 0.25f));
    }

}

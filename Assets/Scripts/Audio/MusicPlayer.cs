using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{
    public AudioClip music, ambience, opera;
    public float musicVolume, ambienceVolume, operaVolume;
    private AudioSource musicSource, ambienceSource, operaSource;
    private bool shouldPlayOpera = false;


    private void Start()
    {

        musicSource = this.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.pitch = 1;
        musicSource.spatialBlend = 0;

        ambienceSource = this.AddComponent<AudioSource>();
        ambienceSource.clip = ambience;
        ambienceSource.loop = true;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.pitch = 1;
        ambienceSource.spatialBlend = 0;
        ambienceSource.Play();


        operaSource = this.AddComponent<AudioSource>();
        operaSource.clip = opera;
        operaSource.loop = true;
        operaSource.volume = 0;
        operaSource.pitch = 1;
        operaSource.spatialBlend = 0;
        operaSource.Play();


    }

    public void StartMusic()
    {
        if (!musicSource.isPlaying) musicSource.Play();
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

        StartCoroutine(LerpAudioSourcePitch(operaSource, 1, 0.5f, 1.5f, false));

    }

    private IEnumerator IFadeInOpera()
    {
        
        if(musicSource == null || operaSource == null) yield break;

        StartCoroutine(FadeInAudioSource(operaSource, operaVolume, 1.5f));
        StartCoroutine(FadeOutAudioSource(musicSource, musicVolume, 1));

        StartCoroutine(LerpAudioSourcePitch(operaSource, 0.5f, 1, 1.5f, true));

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

}

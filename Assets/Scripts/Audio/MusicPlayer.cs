using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{
    public AudioClip music, ambience, opera;
    public float musicVolume, operaVolume;
    private AudioSource musicSource, operaSource;
    private bool shouldPlayOpera = false;


    private void Start()
    {

        musicSource = this.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.pitch = 1;
        musicSource.spatialBlend = 0;
        musicSource.Play();


        operaSource = this.AddComponent<AudioSource>();
        operaSource.clip = opera;
        operaSource.loop = true;
        operaSource.volume = 0;
        operaSource.pitch = 1;
        operaSource.spatialBlend = 0;
        operaSource.Play();


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
        StartCoroutine(CrossfadeAudioSources(musicSource, musicVolume, operaSource, operaVolume, 1.5f));
    }

    private void FadeToOpera()
    {
        StartCoroutine(CrossfadeAudioSources(operaSource, operaVolume, musicSource, musicVolume, 0.5f));
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

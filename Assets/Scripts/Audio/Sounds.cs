using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    public bool muteAll = false;

    public RandomSoundContainer[] soundsOnThisObject;
    private Dictionary<string, AudioSource> loopingAudioSources = new Dictionary<string, AudioSource>();

    private void Start()
    {
        InitializeSounds();
    }

    private void InitializeSounds()
    {

        int i = 0;

        foreach (RandomSoundContainer container in soundsOnThisObject)
        {
            container.id = i++;
        }

    }

    public void SetShouldPlayOnSoundToFalse(int soundID)
    {
        soundsOnThisObject[soundID].SetSouldPlay(false);
    }

    public void PlaySoundContainer(int soundID)
    {
        if (muteAll) return;
        soundsOnThisObject[soundID].PlayRandomSoundOnTarget(transform);
    }

    public void PlaySoundContainer(string containerName)
    {
        if (muteAll) return;
        soundsOnThisObject[GetSoundContainerByName(containerName)].PlayRandomSoundOnTarget(transform);
    }

    public void PlayLoopingContainer(string containerName)
    {
        PlayLoopingContainer(GetSoundContainerByName(containerName));
    }

    public void PlayLoopingContainer(int soundID)
    {

        if (muteAll) return;
        RandomSoundContainer containerToPlay = soundsOnThisObject[soundID];

        if (containerToPlay.soundSettings.loop != true || loopingAudioSources.ContainsKey(containerToPlay.name)) return;

        containerToPlay.PlayLoopingSoundOnTarget(out AudioSource loopingAudioSource, transform);
        loopingAudioSource.volume = 0;
            
        if (loopingAudioSource != null)            
        {
            loopingAudioSources.Add(containerToPlay.name, loopingAudioSource);

            if (containerToPlay.soundSettings.fadeInTime > 0)
            {
                StartCoroutine(IFadeInLoopingSound(containerToPlay.soundSettings.GetVolume(), containerToPlay.soundSettings.fadeInTime, loopingAudioSource));
            }

        }


    }

    public void StopLoopingContainer(int soundID)
    {

        RandomSoundContainer container = soundsOnThisObject[soundID];
        string containerName = container.name;

        if (!loopingAudioSources.ContainsKey(containerName)) return;

        StartCoroutine(IFadeOutLoopingSound(container.soundSettings.GetVolume(), container.soundSettings.fadeOutTime, containerName));

    }

    private IEnumerator IFadeOutLoopingSound(float startValue, float duration, string containerName)
    {
        AudioSource audioSourceToFade = loopingAudioSources[containerName];
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            if (audioSourceToFade == null) break;
            float newVolume = Mathf.Lerp(startValue, 0, timeElapsed / duration);
            audioSourceToFade.volume = newVolume;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (loopingAudioSources.ContainsKey(containerName))
        {
            loopingAudioSources[containerName].Stop();
            Destroy(loopingAudioSources[containerName]);
        }

        loopingAudioSources.Remove(containerName);

        yield return null;

    }

    private IEnumerator IFadeInLoopingSound(float targetVolume, float duration, AudioSource source)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            if (source == null) break;
            float x = timeElapsed / duration;
            float newVolume = Mathf.Lerp(0, targetVolume, x * x * x);
            source.volume = newVolume;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private int GetSoundContainerByName(string name)
    {

        RandomSoundContainer containerToReturn = null;

        foreach (RandomSoundContainer container in soundsOnThisObject)
        {
            if (container.name == name) containerToReturn = container;
        }

        return containerToReturn.id;
    }

}

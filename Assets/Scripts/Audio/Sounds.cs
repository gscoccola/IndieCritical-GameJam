using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    public bool muteAll = false;

    public RandomSoundContainer[] soundsOnThisObject;
    private Dictionary<string, AudioSource> loopingAudioSources = new Dictionary<string, AudioSource>();

    public void SetShouldPlayOnSoundToFalse(int soundID)
    {
        soundsOnThisObject[soundID].SetSouldPlay(false);
    }

    public void PlaySoundContainer(int soundID)
    {
        if (muteAll) return;
        soundsOnThisObject[soundID].PlayRandomSoundOnTarget(transform);
    }

    public void PlayLoopingContainer(int soundID)
    {

        if (muteAll) return;
        RandomSoundContainer containerToPlay = soundsOnThisObject[soundID];

        if (containerToPlay.soundSettings.loop != true) return;

        if (!loopingAudioSources.ContainsKey(containerToPlay.name))
        {

            containerToPlay.PlayLoopingSoundOnTarget(out AudioSource loopingAudioSource, transform);
            if (loopingAudioSource != null) loopingAudioSources.Add(containerToPlay.name, loopingAudioSource);

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

        loopingAudioSources[containerName].Stop();
        Destroy(loopingAudioSources[containerName]);
        loopingAudioSources.Remove(containerName);

        yield return null;

    }

}

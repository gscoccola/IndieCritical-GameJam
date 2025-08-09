using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public RandomSoundContainer[] soundsOnThisObject;
    private Dictionary<string, AudioSource> loopingAudioSources = new Dictionary<string, AudioSource>();
    private List<int> loopingSoundIDs = new List<int>();

    public void PlaySoundContainer(int soundID)
    {
        soundsOnThisObject[soundID].PlayRandomSoundOnTarget(transform);
    }

    public void PlayLoopingContainer(int soundID)
    {

        RandomSoundContainer containerToPlay = soundsOnThisObject[soundID];

        if (containerToPlay.soundSettings.loop != true) return;

        if (!loopingAudioSources.ContainsKey(containerToPlay.name))
        {
            containerToPlay.PlayLoopingSoundOnTarget(out AudioSource loopingAudioSource, transform);
            loopingAudioSources.Add(containerToPlay.name, loopingAudioSource);
            loopingSoundIDs.Add(soundID);
        }

    }

    public void StopLoopingContainer(int soundID)
    {

        string containerName = soundsOnThisObject[soundID].name;

        if (!loopingAudioSources.ContainsKey(containerName)) return;

        loopingAudioSources[containerName].Stop();
        Destroy(loopingAudioSources[containerName]);
        loopingAudioSources.Remove(containerName);

    }

}

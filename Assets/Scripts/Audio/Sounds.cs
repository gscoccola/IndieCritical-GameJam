using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    public bool muteAll = false;

    public RandomSoundContainer[] soundsOnThisObject;
    private Dictionary<string, AudioSource> loopingAudioSources = new Dictionary<string, AudioSource>();


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
            print("Looping " + containerToPlay.name);

            containerToPlay.PlayLoopingSoundOnTarget(out AudioSource loopingAudioSource, transform);
            loopingAudioSources.Add(containerToPlay.name, loopingAudioSource);

        }

    }

    public void StopLoopingContainer(int soundID)
    {

        string containerName = soundsOnThisObject[soundID].name;
        print("Stopping " + containerName);

        if (!loopingAudioSources.ContainsKey(containerName)) return;

        print("Stopped " + containerName);


        loopingAudioSources[containerName].Stop();
        Destroy(loopingAudioSources[containerName]);
        loopingAudioSources.Remove(containerName);

    }

}

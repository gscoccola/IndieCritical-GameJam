using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class HeartBeatingSound : MonoBehaviour
{

    public RandomSoundContainer heartBeatingSounds;
    public float beatInterval_sec;
    private float actualVolume;

    private bool shouldBeat;

    public void StartBeating()
    {
        shouldBeat = true;
        StartCoroutine(IHeartBeatCoroutine());
        StartCoroutine(IFadeInHeartBeatVolume());
    }

    private IEnumerator IFadeInHeartBeatVolume()
    {
        float timeElapsed = 0;

        while (timeElapsed < heartBeatingSounds.soundSettings.fadeInTime)
        {
            float newVolume = Mathf.Lerp(0, heartBeatingSounds.soundSettings.GetVolume(), timeElapsed / heartBeatingSounds.soundSettings.fadeInTime);
            actualVolume = newVolume;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator IHeartBeatCoroutine()
    {
        heartBeatingSounds.PlayRandomSoundOnTarget(transform, actualVolume);
        yield return new WaitForSeconds(beatInterval_sec);

        if (shouldBeat) StartCoroutine(IHeartBeatCoroutine());

    }

    public void StopBeating()
    {
        shouldBeat = false;
        StopAllCoroutines();
    }


}

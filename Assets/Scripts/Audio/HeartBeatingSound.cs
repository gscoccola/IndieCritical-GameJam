using System.Collections;
using UnityEngine;

public class HeartBeatingSound : MonoBehaviour
{

    public RandomSoundContainer heartBeatingSounds;
    public float beatInterval_sec;

    private bool shouldBeat;

    public void StartBeating()
    {
        shouldBeat = true;
        StartCoroutine(IHeartBeatCoroutine());
    }

    private IEnumerator IHeartBeatCoroutine()
    {
        heartBeatingSounds.PlayRandomSoundOnTarget(transform);
        yield return new WaitForSeconds(beatInterval_sec);

        if (shouldBeat) StartCoroutine(IHeartBeatCoroutine());

    }

    public void StopBeating()
    {
        shouldBeat = false;
        StopAllCoroutines();
    }


}

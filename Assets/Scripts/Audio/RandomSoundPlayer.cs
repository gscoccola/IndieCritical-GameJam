using System.Collections;
using UnityEngine;


public class RandomSoundPlayer : MonoBehaviour
{
    private bool shouldPlaySounds = false;
    public Vector2 randomTimeRange;
    public RandomSoundContainer soundContainer;
    private int maxVoices = 4;

    public void StartSounds()
    {
        shouldPlaySounds = true;
        StartCoroutine(PlayRandomSounds());
    }

    public void Oneshot()
    {
        soundContainer.PlayRandomSoundOnTarget(transform);
    }

    public void StopSounds()
    {
        shouldPlaySounds = false;
        StopAllCoroutines();

    }

    private IEnumerator PlayRandomSounds()
    {

        if (shouldPlaySounds || soundContainer != null)
        {
            yield return new WaitForSeconds(Random.Range(randomTimeRange.x, randomTimeRange.y));

            StartCoroutine(PlayRandomSounds());
            soundContainer.PlayRandomSoundOnTarget(transform);
        }

        yield return null;
    }


}

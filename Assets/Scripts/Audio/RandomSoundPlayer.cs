using System.Collections;
using UnityEngine;


public class RandomSoundPlayer : MonoBehaviour
{
    private bool shouldPlaySounds = false;
    public Vector2 randomTimeRange;
    public RandomSoundContainer soundContainer;

    public void StartSounds()
    {
        shouldPlaySounds = true;
        StartCoroutine(PlayRandomSounds());
    }

    public void StopSounds()
    {
        shouldPlaySounds = false;
        StopAllCoroutines();

    }

    private IEnumerator PlayRandomSounds()
    {
        yield return new WaitForSeconds(Random.Range(randomTimeRange.x, randomTimeRange.y));

        if (shouldPlaySounds || soundContainer != null)
        {
            StartCoroutine(PlayRandomSounds());
            soundContainer.PlayRandomSoundOnTarget(transform);
        }
    }


}

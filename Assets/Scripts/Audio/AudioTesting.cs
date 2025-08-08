using UnityEngine;

public class AudioTesting : MonoBehaviour
{

    public AudioClip testClip, testClipEQed;
    private AudioSource audioSource;
    private bool flip = true;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = testClip;
        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            AudioClip clip = flip ? testClipEQed : testClip;

            //use time
            float time = audioSource.time;
            audioSource.clip = clip;
            audioSource.time = time;

            //use time samples
            //int samplePosition = audioSource.timeSamples;
            //audioSource.clip = clip;
            //audioSource.timeSamples = samplePosition;

            audioSource.pitch = 1;
            audioSource.Play();
            flip = !flip;

        }
    }

}

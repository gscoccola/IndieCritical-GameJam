using UnityEngine;

public class AudioTesting : MonoBehaviour
{

    public AudioClip testClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = testClip;
        audioSource.Play();
    }

}

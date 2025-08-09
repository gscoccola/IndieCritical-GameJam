using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RandomSoundContainer
{

    public string name;
    [SerializeField] private bool mute = false;
    [SerializeField] private AudioClip[] audioClips;
    public SoundSettings soundSettings;
    private bool shouldPlay = true;

    public void PlayRandomSoundOnTarget(Transform target)
    {
        if (!shouldPlay || mute) return;

        int randInt = Random.Range(0, audioClips.Length);

        AudioSource source = target.AddComponent<AudioSource>();
        AudioClip clip = audioClips[randInt];

        source.clip = clip;
        ApplySoundSettings(soundSettings, source);
        source.Play();

        Object.Destroy(source, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));

    }

    public void SetSouldPlay(bool b)
    {
        shouldPlay = b;
    }

    public void PlayLoopingSoundOnTarget(out AudioSource sourcePlayingOn, Transform target)
    {

        if (!shouldPlay || mute) {

            sourcePlayingOn = null;    
            return; 
        
        }

        AudioSource source = target.AddComponent<AudioSource>();
        AudioClip clip = audioClips[0];

        source.clip = clip;
        ApplySoundSettings(soundSettings, source);
        source.Play();

        sourcePlayingOn = source;

    }

    private void ApplySoundSettings(SoundSettings soundSettings, AudioSource audioSource)
    {

        audioSource.priority = 128;
        audioSource.loop = soundSettings.loop;
        audioSource.volume = soundSettings.GetVolume();
        audioSource.pitch = soundSettings.GetPitch();
        audioSource.spatialBlend = 1;
        audioSource.spread = 110;
        audioSource.minDistance = 10;
        audioSource.maxDistance = 30;

    }


}


/// <summary>
/// The settings for Sounds, SoundContainers, and eventually Music. Random values/ranges will not be applied if the corresponding bools are not true.
/// </summary>
[System.Serializable]
public class SoundSettings
{

    [Header("General Settings")]

    //[Tooltip("Sets the prioroity of the AudioSource. The lower the number the higher the priority when Unity decides which sounds to play or cull.")]
    //[Range(0, 256)] public int priority = 128;

    [Tooltip("Set if the Sound should play again after playback.")]
    public bool loop;
    public float fadeOutTime = 0.5f;

    [Header("Volume Settings")]

    //[Tooltip("Set the volume to play the Sound at. This setting will be ignored on playback if Randomize Volume is true.")]
    //[Range(0.001f, 1)] public float volume = 1;

    [Tooltip("Set if the Volume of this Sound should be randomized.")]
    public bool randomizeVolume;

    [Tooltip("Set the range of volume randomization for this Sound. X value is the minimum volume, and the Y value is the max. These settings will be ignored on playback if Randomize Volume is false.")]
    public Vector2 volumeRange = new Vector2(1, 1);

    [Header("Pitch Settings")]

    //[Tooltip("Set the pitch to play the Sound at. This setting will be ignored on playback if Randomize Pitch is true.")]
    //[Range(0.001f, 3)] public float pitch = 1;

    [Tooltip("Set if the pitch of this Sound should be randomized.")]
    public bool randomizePitch;

    [Tooltip("Set the range of pitch randomization for this Sound. X value is the minimum pitch, and the Y value is the max. These settings will be ignored on playback if Randomize Pitch is false.")]
    public Vector2 pitchRange = new Vector2(1, 1);

    //[Header("Misc Settings")]

    //[Tooltip("Set the Spacial Blend for this Sound. 0 = 2D playback, 1 = full 3D playback.")]
    //[Range(0, 1)] public float spatialBlend = 0.5f;
    ////[SerializeField] private bool _ignorePause;

    //[Tooltip("Set the Spread for this Sound.")]
    //[Range(0, 360)] public float spread = 0f;

    //[Tooltip("Set the cooldown for this Sound.")]
    //public float bufferTime = 0f;

    ////The last triggered time of this Sound. It is set wierd because I set it this way initially and would be a big task to correct it now for some reason - Kyle
    //[HideInInspector] public float lastTriggeredTime = 50000f;

    public float GetVolume()
    {
        return randomizeVolume ? Random.Range(volumeRange.x, volumeRange.y) : volumeRange.x;
    }

    public float GetPitch()
    {
        return randomizePitch ? Random.Range(pitchRange.x, pitchRange.y) : pitchRange.x;
    }

}

public class SoundClasses 
{
   



}

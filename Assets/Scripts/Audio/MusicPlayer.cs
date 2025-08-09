using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{
    private AudioLowPassFilter lowPassFilter => GetComponent<AudioLowPassFilter>();

    public void ToggleMuffle(bool value)
    {
        lowPassFilter.cutoffFrequency = value ? 400f : 22000f;
    }
}

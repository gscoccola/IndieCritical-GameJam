using UnityEngine;

public class TapeRecorder : MonoBehaviour, IPowerable
{
    public bool IsPowered { get; private set; }

    private float initialScale;

    private void Start()
    {
        initialScale = transform.localScale.x;
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {

        if (statusEffect != StatusEffect.BloodPowered) return;
        //MusicPlayer.Instance.ToggleMuffle(value);
        MusicPlayer.Instance.ToggleMusic(value);
        IsPowered = value;
        transform.GetChild(0).gameObject.SetActive(value);
    }
}

using UnityEngine;

public class TapeRecorder : MonoBehaviour, IPowerable
{
    public bool IsPowered { get; private set; }

    private float initialScale;

    private void Start()
    {
        initialScale = transform.localScale.x;
    }

    private void Update()
    {
        if (IsPowered)
        {
            transform.localScale = (Mathf.PingPong(Time.time * 0.2f, 0.2f) * Vector3.one + Vector3.one) * initialScale;
        }
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {
        if (statusEffect != StatusEffect.BloodPowered) return;
        MusicPlayer.Instance.ToggleMuffle(value);
        IsPowered = value;
    }
}

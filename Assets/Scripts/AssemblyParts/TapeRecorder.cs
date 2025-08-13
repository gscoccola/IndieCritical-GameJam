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
        
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {

        if (statusEffect != StatusEffect.BloodPowered) return;
        if (!value && IsPowered) Debug.Log("music stopped??");
        MusicPlayer.Instance.ToggleMuffle(value);
        IsPowered = value;
        transform.GetChild(0).gameObject.SetActive(value);
    }
}

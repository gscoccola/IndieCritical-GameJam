using UnityEngine;

public class TapeRecorder : MonoBehaviour, IPowerable
{
    public bool IsPowered { get; private set; }

    private float initialScale;
    private Animator _animator;

    private void Start()
    {
        initialScale = transform.localScale.x;
        _animator = GetComponent<Animator>();
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {

        if (statusEffect != StatusEffect.BloodPowered) return;
        _animator.SetBool("IsPowered", value);
        MusicPlayer.Instance.ToggleMuffle(value);
        IsPowered = value;
        transform.GetChild(0).gameObject.SetActive(value);
    }
}

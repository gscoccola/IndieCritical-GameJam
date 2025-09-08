using UnityEngine;

public class TapeRecorder : MonoBehaviour, IPowerable
{
    public bool IsPowered { get; private set; }


    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {

        if (statusEffect != StatusEffect.BloodPowered) return;
        //MusicPlayer.Instance.ToggleMuffle(value);
        _animator.SetBool("IsPowered", value);
        MusicPlayer.Instance.ToggleMusic(value);
        IsPowered = value;
        transform.GetChild(0).gameObject.SetActive(value);

    }
}

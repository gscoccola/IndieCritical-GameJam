using UnityEngine;

public class Heart : MonoBehaviour, IPowerable
{
    private Assemblable _assemblable => GetComponent<Assemblable>();

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
        if (!value || IsPowered|| statusEffect != StatusEffect.Electrified) return;
        IsPowered = true;
        _assemblable.StatusEffectAffinities[StatusEffect.BloodPowered] = StatusEffectAffinity.Source;
    }
}

using UnityEngine.Events;
using UnityEngine;

public class Heart : MonoBehaviour, IPowerable
{
    private Assemblable _assemblable => GetComponent<Assemblable>();

    public bool IsPowered { get; private set; }

    private float initialScale;

    public UnityEvent OnBecomePowered;

    private void Start()
    {
        initialScale = transform.localScale.x;
    }


    public void SetPowered(StatusEffect statusEffect, bool value)
    {
        if (!value || IsPowered|| statusEffect != StatusEffect.Electrified) return;
        ObjectivesManager.Instance.SetPower();
        IsPowered = true;
        GetComponent<Animator>().SetBool("IsPumping", true);
        OnBecomePowered?.Invoke();
        transform.GetChild(0).GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.BloodPowered] = StatusEffectAffinity.Source;
        GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.Electrified] = StatusEffectAffinity.Insulator;
    }
}

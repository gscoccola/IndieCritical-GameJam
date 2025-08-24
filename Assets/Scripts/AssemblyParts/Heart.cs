using UnityEngine.Events;
using UnityEngine;

public class Heart : MonoBehaviour, IPowerable, IActivable
{
    private Assemblable _assemblable;
    [SerializeField] GameObject _sparks;

    public bool IsPowered { get; private set; }

    public UnityEvent OnBecomePowered;

    private void Start()
    {
        _assemblable = GetComponent<Assemblable>();
    }


    public void SetPowered(StatusEffect statusEffect, bool value)
    {
        if (!value || IsPowered|| statusEffect != StatusEffect.Electrified) return;
        Instantiate(_sparks, transform.position - 0.1f * Vector3.forward, Quaternion.identity);
        SubobjectivesManager.Instance.SetPower();
        IsPowered = true;
        GetComponent<Animator>().SetBool("IsPumping", true);
        OnBecomePowered?.Invoke();
        transform.GetChild(0).GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.BloodPowered] = StatusEffectAffinity.Source;
        GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.Electrified] = StatusEffectAffinity.Insulator;
    }

    public void SetActive(bool value)
    {

        if (!value) return;
        CompletionInfo.Instance.SetCheckpoint();
        
    }
}

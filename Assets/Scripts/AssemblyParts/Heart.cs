using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour, IPowerable, IActivable
{
    private Assemblable _assemblable;
    private Animator _animator;
    private bool _isAwake;
    private Rigidbody2D _rb;
    [SerializeField] GameObject _sparks;

    public bool IsPowered { get; private set; }

    public UnityEvent OnBecomePowered;

    private void Start()
    {
        _assemblable = GetComponent<Assemblable>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }


    public void SetPowered(StatusEffect statusEffect, bool value)
    {
        if (!value || IsPowered|| statusEffect != StatusEffect.Electrified) return;
        Instantiate(_sparks, transform.position - 0.1f * Vector3.forward, Quaternion.identity);
        SubobjectivesManager.Instance.SetPower();
        IsPowered = true;
        _animator.SetBool("IsPumping", true);
        OnBecomePowered?.Invoke();
        transform.GetChild(0).GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.BloodPowered] = StatusEffectAffinity.Source;
        GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.Electrified] = StatusEffectAffinity.Insulator;
    }

    public void SetActive(bool value)
    {

        if (!value) return;
        CompletionInfo.Instance.SetCheckpoint();
        
    }

    public void TriggerTransform()
    {
        StartCoroutine(ITriggerAwake());
    }

    private IEnumerator ITriggerAwake()
    {
        _isAwake = true;
        transform.position -= 4f * Vector3.forward;
        _animator.SetBool("IsPumping", false);
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("Awake");
        yield return new WaitForSeconds(3.6f);
        _animator.SetTrigger("Walk");
        _rb.linearVelocity = new Vector2(-1f, -3.5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isAwake || !IsPowered) return;
        if (col.gameObject.GetComponent<Mouth>() != null)
        {
            TriggerTransform();
            return;
        }
    }
}

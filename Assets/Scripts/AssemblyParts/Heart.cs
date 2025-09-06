using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heart : MonoBehaviour, IPowerable, IActivable
{
    private Assemblable _assemblable;
    [SerializeField] Assemblable _extensionAssemblable;
    private Animator _animator;
    private bool _isAwake;
    private Rigidbody2D _rb;
    [SerializeField] Sprite[] _splatterSprites;
    [SerializeField] GameObject _sparks;
    [SerializeField] GameObject _bloodSplatter;
    [SerializeField] bool _triggerAwake;

    public bool IsPowered { get; private set; }

    public UnityEvent OnBecomePowered;

    private void Update()
    {
        if (_triggerAwake)
        {
            _triggerAwake = false;
            TriggerTransform();
        }
    }

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
        _extensionAssemblable.StatusEffectAffinities[StatusEffect.BloodPowered] = StatusEffectAffinity.Source;
        _assemblable.StatusEffectAffinities[StatusEffect.Electrified] = StatusEffectAffinity.Insulator;
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
        _animator.SetBool("IsPumping", false);
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("Awake");
        yield return new WaitForSeconds(3.6f);
        _rb.linearVelocity = new Vector2(1.6f, -0.4f);
        _animator.SetTrigger("Walk");
        StartCoroutine(BloodSplatters());
        yield return new WaitForSeconds(1.2f);
        transform.localScale = new Vector3(-1f, 1f, 1f);
        //_rb.linearVelocity = new Vector2(-1.8f, -0.7f);
        _rb.linearVelocity = new Vector2(-1.8f, -0.5f);
        SubobjectivesManager.Instance.TriggerEnding(Endings.Freed);
    }

    private IEnumerator BloodSplatters()
    {
        while (true)
        {
            Instantiate(_bloodSplatter, transform.position +
                new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), -2f), Quaternion.identity)
                .GetComponent<SpriteRenderer>().sprite = _splatterSprites[Random.Range(0, _splatterSprites.Length)];
            yield return new WaitForSeconds(0.5f);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isAwake || !IsPowered) return;
        if (col.gameObject.GetComponent<Mouth>() != null)
        {
            TriggerTransform();
            col.transform.parent.gameObject.GetComponent<Centipede>().TriggerAbsorption();
            return;
        }
    }*/
}

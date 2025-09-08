using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour, IPowerable
{
    private Centipede _centipede;
    public Sounds _sounds;
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject _eatingParticles;
    [SerializeField] GameObject _electricitySprite;
    Rigidbody2D _rb;
    private Vector3 _initialPosition;
    private ForceDraggable _draggable;
    [SerializeField] private Eye _eye;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private float _eatDisplacement = 0.5f;
    public bool IsAbsorbed;

    private bool _isStunned;
    private bool _isSelected;
    private bool _isAsborbed;
    private bool _wasMoved;
    private bool _spriteReturning;
    private bool _isBlended;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
        _sounds = GetComponentInParent<Sounds>();
        _rb = GetComponent<Rigidbody2D>();
        _draggable = GetComponent<ForceDraggable>();
        _initialPosition = transform.position;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        
        if (_wasMoved && !_isSelected && !_isStunned && _draggable.IsSelectable)
        {
            _rb.linearVelocity = (Vector3.MoveTowards(_rb.linearVelocity, _initialPosition - transform.position, 2f * Time.fixedDeltaTime));
            if (Vector3.Distance(transform.position, _initialPosition) < 0.1f && Mathf.Abs(_rb.rotation - 180f) < 5f)
            {
                _wasMoved = false;
                _centipede._sounds.StopLoopingContainer("Orobourus_Movement");
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }

            _rb.angularVelocity = -_rb.rotation + 180f;
        }
    }

    private void Update()
    {
        if (!_spriteReturning) return;
        _spriteTransform.localPosition = Vector3.MoveTowards(_spriteTransform.localPosition, Vector3.zero, 0.5f * Time.deltaTime);
        if (_spriteTransform.localPosition.magnitude < 0.01f) _spriteReturning = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isStunned) return;
        if (col.gameObject.GetComponent<Screw>() != null)
        {

            TurnHead(col);

            if (col.gameObject.GetComponent<Assemblable>().CurrentStatusEffects[StatusEffect.Electrified])
            {
                return;
            }

            col.GetComponent<Draggable>().IsSelectable = false;
            col.transform.position += 1000f * Vector3.up;
            StartCoroutine(IGrow());
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("PhillipsScrewHead"))
        {
            col.gameObject.GetComponentInParent<Screwdriver>().SwitchHead();
            col.gameObject.GetComponentInParent<Draggable>().IsSelected = false;
            TurnHead(col);
            StartCoroutine(IGrow(2));
        }

    }

    private void Stun()
    {
        if (_isBlended) return;
        _centipede._sounds.StopLoopingContainer("Orobourus_Movement");
        Instantiate(_electricitySprite, transform.position, Quaternion.identity);
        _eye.IsStunned = true;
        _isStunned = true;
        _animator.SetBool("Grabbing", false);
        _animator.SetBool("Stunned", true);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        //_rb.constraints = RigidbodyConstraints2D.None;
        StartCoroutine(IDestun());
        _sounds.PlaySoundContainer("Shock");
    }

    private void TurnHead(Collider2D col)
    {
        _spriteTransform.localPosition += -(col.transform.position - transform.position).Flatten().normalized * _eatDisplacement;
        _spriteReturning = true;
    }

    private IEnumerator IGrow(int number = 1)
    {
        _sounds.PlaySoundContainer("Eat");

        Instantiate(_eatingParticles, transform.position - 0.1f * Vector3.forward, Quaternion.identity);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(0.4f);
            _centipede.AddSegment();
        }

    }

    public void SetGrabbing(bool value)
    {
        if (_isBlended) return;
        _isSelected = value;
        if (_isStunned)
        {
            _wasMoved = true;
            return;
        }

        if (value) _centipede._sounds.PlaySoundContainer_TransformOverride("Grab", transform); 

        //_rb.constraints = value ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        _rb.bodyType = value ? RigidbodyType2D.Static : RigidbodyType2D.Kinematic;
        _animator.SetBool("Grabbing", value);
        if (!_wasMoved) return;

        if (value) _centipede._sounds.StopLoopingContainer("Orobourus_Movement");
        else _centipede._sounds.PlayLoopingContainer("Orobourus_Movement");
    }

    public void SetBlended()
    {
        if (_isStunned) DeStun();
        _isBlended = true;
        _centipede._sounds.StopLoopingContainer("Orobourus_Movement");
        _animator.SetBool("Grabbing", false);
    }

    private IEnumerator IDestun()
    {
        yield return new WaitForSeconds(5f);
        DeStun();
    }

    private void DeStun()
    {
        StopAllCoroutines();
        _isStunned = false;
        _eye.IsStunned = false;
        //_rb.constraints = _isSelected ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        //_rb.bodyType = _isSelected ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        _animator.SetBool("Stunned", false);
        if (_isAsborbed)
        {
            return;
        }
        if (!_isSelected) _centipede._sounds.PlayLoopingContainer("Orobourus_Movement");
        _animator.SetBool("Grabbing", _isSelected);
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.linearVelocity = Vector2.zero;
    }

    public void SetPowered(StatusEffect statusEffect, bool value)
    {
        if (value && !_isStunned && statusEffect == StatusEffect.Electrified)
        {
            Stun();

        }

        else if (value && !_isAsborbed && statusEffect == StatusEffect.BloodPowered)
        {
            _isAsborbed = true;
            _wasMoved = false;
            _centipede.StartCoroutine(_centipede.IAbsorbed());
        }
    }
}

using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    private Centipede _centipede;
    private Sounds _sounds;
    private Animator _animator;
    [SerializeField] GameObject _particles;
    Rigidbody2D _rb;
    private Vector3 _initialPosition;
    private ForceDraggable _draggable;

    private bool _isStunned;
    private bool _isSelected;
    private bool _wasMoved;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
        _sounds = GetComponentInParent<Sounds>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _draggable = GetComponent<ForceDraggable>();
        _initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_rb.rotation > 190f)
        {
            _rb.AddTorque(-60f * Time.fixedDeltaTime);
        }
        else if (_rb.rotation < 170f)
        {
            _rb.AddTorque(60f * Time.fixedDeltaTime);
        }
        if (_wasMoved && !_isSelected && !_isStunned &&  _draggable.IsSelectable)
        {
            _rb.MovePosition(Vector3.MoveTowards(transform.position, _initialPosition, 2f * Time.fixedDeltaTime));
            if (Vector3.Distance(transform.position, _initialPosition) < 0.1f)
            {
                _wasMoved = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isStunned) return;
        if (col.gameObject.GetComponent<Screw>() != null)
        {

            TurnHead(col);

            if (col.gameObject.GetComponent<Assemblable>().CurrentStatusEffects[StatusEffect.Electrified])
            {
                _isStunned = true;
                _animator.SetBool("Grabbing", false);
                _animator.SetBool("Stunned", true);
                _rb.constraints = RigidbodyConstraints2D.None;
                StartCoroutine(IDestun());
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

    private void TurnHead(Collider2D col)
    {
        if (col.transform.position.y > transform.position.y)
        {
            _rb.AddTorque(300f);
        }
        else
        {
            _rb.AddTorque(-300f);
        }
    }

    private IEnumerator IGrow(int number = 1)
    {
        _sounds.PlaySoundContainer("Eat");

        Instantiate(_particles, transform.position - 0.1f * Vector3.forward, Quaternion.identity);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(0.4f);
            _centipede.AddSegment();
        }

    }

    public void SetGrabbing(bool value)
    {
        _isSelected = value;
        if (_isStunned)
        {
            _wasMoved = true;
            return;
        }
        _rb.constraints = value ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        _animator.SetBool("Grabbing", value);
    }

    private IEnumerator IDestun()
    {
        yield return new WaitForSeconds(5f);
        _isStunned = false;
        _rb.constraints = _isSelected ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        _animator.SetBool("Stunned", false);
        _animator.SetBool("Grabbing", _isSelected);
    }

    public void NotifyMovement()
    {
        if (_isStunned) ;
    }
}

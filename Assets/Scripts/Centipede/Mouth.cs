using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    private Centipede _centipede;
    private Sounds _sounds;
    private Animator _animator;
    [SerializeField] GameObject _particles;
    [SerializeField] Rigidbody2D _rb;

    private bool _isStunned;
    private bool _isSelected;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
        _sounds = GetComponentInParent<Sounds>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isStunned) return;
        if (col.gameObject.GetComponent<Screw>() != null)
        {

            TurnHead(col);
            col.GetComponent<Draggable>().IsSelectable = false;

            if (col.gameObject.GetComponent<Assemblable>().CurrentStatusEffects[StatusEffect.Electrified])
            {
                _isStunned = true;
                _animator.SetBool("Grabbing", false);
                _rb.constraints = RigidbodyConstraints2D.None;
                StartCoroutine(IDestun());
                return;
            }

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
        if (_isStunned) return;
        _rb.constraints = value ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        _animator.SetBool("Grabbing", value);
    }

    private IEnumerator IDestun()
    {
        yield return new WaitForSeconds(2f);
        _isStunned = false;
        _rb.constraints = _isSelected ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        _animator.SetBool("Grabbing", _isSelected);
    }
}

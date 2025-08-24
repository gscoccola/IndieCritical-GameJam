using System.Collections;
using UnityEngine;

public class Gremlin : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _buttonSeekDistance = 5f;

    private Rigidbody2D _rb => GetComponent<Rigidbody2D>();
    private Draggable _draggable;
    private Assemblable _assemblable => GetComponent<Assemblable>();
    private Vector3 _speedTarget;
    public bool _isUncovered;
    public bool _isStunned;
    public bool _isSelected;
    public bool _usingCalculator;
    //private Calculator _calculator;
    private GremlinBox _box;
    private Vector3 _lastPos;
    private Animator _animator;
    private Sounds _sounds;
    private Button _button;
    private bool _hasPressedButton;

    private void Start()
    {
        _isUncovered = false;
        _speedTarget = Random.insideUnitCircle.normalized * _speed;
        _draggable = GetComponent<Draggable>();
        _box = FindAnyObjectByType<GremlinBox>();
        _draggable.IsFixed = true;
        _animator = GetComponent<Animator>();
        _sounds = GetComponent<Sounds>();
        _button = FindAnyObjectByType<Button>();
    }

    private void FixedUpdate()
    {
        _animator.SetBool("IsCalculating", _usingCalculator);
        if (!_hasPressedButton && (transform.position.Flatten() - _button.transform.position.Flatten()).magnitude < 0.1f)
            _hasPressedButton = true;
        if (_usingCalculator || _isSelected || _isStunned) _rb.linearVelocity = Vector2.zero;
        else if (_isUncovered)
            _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, _speedTarget, _acceleration * Time.deltaTime);
        if (!_isSelected) _animator.SetBool("IsWalking", (_lastPos - transform.position).magnitude > Time.deltaTime * 0.2f * _speed);
        _lastPos = transform.position;
    }

    private IEnumerator IChangeDirection()
    {
        while (true)
        {
            
            if (!_hasPressedButton && (transform.position.Flatten() - _button.transform.position.Flatten()).magnitude < _buttonSeekDistance)
            {
                _speedTarget = (_button.transform.position.Flatten() - transform.position.Flatten()).normalized * _speed;
            }
            else
            {
                _speedTarget = Random.insideUnitCircle.normalized * _speed;
                _speedTarget = new Vector3(_speedTarget.x, _speedTarget.y * 0.5f, _speedTarget.z);
            }
            transform.localScale = new Vector3(Mathf.Sign(-_speedTarget.x), 1f, 1f);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    public void SetActive()
    {
        _draggable.IsSelectable = true;
        _isUncovered = true;
        _isStunned = true;
        _draggable.IsFixed = false;
        _animator.SetBool("IsStunned", true);
        _sounds.PlayLoopingContainer("GremlinStunned");
    }

    public void OnSelect()
    {
        if (_isStunned) transform.position -= 0.2f * Vector3.forward;
        StopAllCoroutines();
        _isStunned = false;
        _isSelected = true;
        _usingCalculator = false;
        _animator.SetBool("IsStunned", false); 
        _animator.SetBool("IsWalking", false); 
        _sounds.StopLoopingContainer("GremlinStunned");
    }

    public void OnDeselect()
    {
        _isSelected = false;
        StartCoroutine(IChangeDirection());
    }

    public void OnJiggle()
    {
        if (!_isUncovered) { _box.HeadHide(); return; }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Calculator"))
        {
            _usingCalculator = true;
            SubobjectivesManager.Instance.SetCPU(true);
            col.gameObject.GetComponent<Calculator>().SetPowered(true);
            transform.position = new Vector3( col.transform.position.x, col.transform.position.y, transform.position.z);
            StartCoroutine(IBeginCalculating());
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            col.GetComponent<Button>().OnActivate();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Glass"))
        {
            _draggable.IsSelected = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Calculator"))
        {
            _usingCalculator = false;
            SubobjectivesManager.Instance.SetCPU(false);
            col.gameObject.GetComponent<Calculator>().SetPowered(false);
        }
    }



    private IEnumerator IBeginCalculating()
    {
        _draggable.IsSelectable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        _draggable.IsSelectable = true;
    }
}

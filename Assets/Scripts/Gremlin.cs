using System.Collections;
using UnityEngine;

public class Gremlin : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 5f;

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

    private void Start()
    {
        _isUncovered = false;
        _speedTarget = Random.insideUnitCircle.normalized * _speed;
        StartCoroutine(IChangeDirection());
        _draggable = GetComponent<Draggable>();
        _box = FindAnyObjectByType<GremlinBox>();
        _draggable.IsFixed = true;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _animator.SetBool("IsCalculating", _usingCalculator);
        if (_usingCalculator || _isSelected || _isStunned) _rb.linearVelocity = Vector2.zero;
        else if (_isUncovered)
            _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, _speedTarget, _acceleration * Time.deltaTime);
        _animator.SetBool("IsWalking", (_lastPos - transform.position).magnitude > Time.deltaTime * 0.2f * _speed);
        _lastPos = transform.position;
    }

    private IEnumerator IChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            _speedTarget = Random.insideUnitCircle.normalized * _speed;
            _speedTarget = new Vector3(_speedTarget.x, _speedTarget.y * 0.5f, _speedTarget.z);
            transform.localScale = new Vector3(Mathf.Sign(-_speedTarget.x), 1f, 1f);
        }
    }

    public void SetActive()
    {
        _draggable.IsSelectable = true;
        _isUncovered = true;
        _isStunned = true;
        _draggable.IsFixed = false;
    }

    public void OnSelect()
    {
        _isStunned = false;
        _isSelected = true;
        _usingCalculator = false;
    }

    public void OnDeselect()
    {
        _isSelected = false;
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
            ObjectivesManager.Instance.SetCPU(true);
            /*_calculator = col.gameObject.GetComponent<Calculator>();*/
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
            ObjectivesManager.Instance.SetCPU(false);
            /*_calculator = null;*/
        }
    }



    private IEnumerator IBeginCalculating()
    {
        _draggable.IsSelectable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        _draggable.IsSelectable = true;
    }
}

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

    private void Start()
    {
        _isUncovered = false;
        _speedTarget = Random.insideUnitCircle.normalized * _speed;
        StartCoroutine(IChangeDirection());
        _draggable = GetComponent<Draggable>();
        _draggable.IsSelectable = false;
    }

    private void Update()
    {
        if (_isUncovered && !_isSelected && !_isStunned)
        _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, _speedTarget, _acceleration * Time.deltaTime);
    }

    private IEnumerator IChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            _speedTarget = Random.insideUnitCircle.normalized * _speed;
            _speedTarget = new Vector3(_speedTarget.x, _speedTarget.y * 0.5f, _speedTarget.z);
        }
    }

    public void SetActive()
    {
        _draggable.IsSelectable = true;
        _isUncovered = true;
        _isStunned = true;
    }

    public void OnSelect()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        _isStunned = false;
        _isSelected = true;
    }

    public void OnDeselect()
    {
        _isSelected = false;
    }
}

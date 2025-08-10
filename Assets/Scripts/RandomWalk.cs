using System.Collections;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _acceleration = 5f;

    private Rigidbody2D _rb => GetComponent<Rigidbody2D>();
    private Draggable _draggable => GetComponent<Draggable>();
    private Assemblable _assemblable => GetComponent<Assemblable>();
    private Vector3 _speedTarget;
    public bool _isUncovered;

    private void Start()
    {
        _speedTarget = Random.insideUnitCircle.normalized * _speed;
        StartCoroutine(IChangeDirection());
    }

    private void Update()
    {
        _isUncovered = _assemblable.IsUncovered;
        if (_isUncovered && !_draggable.IsSelected)
        _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, _speedTarget, _acceleration * Time.deltaTime);
    }

    private IEnumerator IChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            _speedTarget = Random.insideUnitCircle.normalized * _speed;
        }
    }

}

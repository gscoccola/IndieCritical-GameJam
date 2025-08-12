using System.Collections;
using UnityEngine;

public class GremlinBox : MonoBehaviour
{
    [SerializeField] float _maxDisplacement = 0.5f;

    [SerializeField] private Transform _cover;
    [SerializeField] private Transform _background;
    private Draggable _coverDraggable;

    public float _displacement;
    public bool IsUnlocked;
    private bool _isReturning;
    private float _newRelDis;
    private float _returnTime;
    private float _returnSpeed = 0.2f;

    private void Start()
    {
        _coverDraggable = _cover.GetComponent<Draggable>();
        _returnTime = _maxDisplacement / _returnSpeed;
    }

    private void Update()
    {
        _displacement = _background.position.y -_cover.position.y;
        if (_displacement > _maxDisplacement && !IsUnlocked)
        {
            StartCoroutine(ISlideBack());
        }

        if (_isReturning)
        {
            _newRelDis = Mathf.MoveTowards(_newRelDis, 0, Time.deltaTime * _returnSpeed);
            _cover.position = transform.position - _newRelDis * Vector3.up;
        }
    }

    private IEnumerator ISlideBack()
    {
        _newRelDis = _background.position.y - _cover.position.y;
        _coverDraggable.IsSelectable = false;
        yield return new WaitForSeconds(0.3f);
        _isReturning = true;
        yield return new WaitForSeconds(_returnTime);
        _isReturning = false;
        _coverDraggable.IsSelectable = true;
    }
}

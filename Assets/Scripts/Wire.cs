using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private Transform _wireOrigin;
    [SerializeField] private float _maxLength;
    [SerializeField] private float _lenghtAtRest;
    [SerializeField] private LimitType _Xlimit;
    [SerializeField] private LimitType _Ylimit;
    private LineRenderer _lineRenderer;
    private bool _isSelected;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _wireOrigin.position = new Vector3(_wireOrigin.position.x, _wireOrigin.position.y, transform.position.z);
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, new Vector3(_wireOrigin.position.x, _wireOrigin.position.y, transform.position.z));
        _lineRenderer.SetPosition(1, transform.position);
    }

    public void OnMouseDrag()
    {
        if (!_isSelected) return;
        MoveToMousePos(_maxLength);
    }

    private void MoveToMousePos(float lenght)
    {
        Vector3 mouseRelativePos = _camera.ScreenToWorldPoint(Input.mousePosition) - _wireOrigin.position;

        mouseRelativePos = new Vector3(
             _Xlimit == LimitType.Min ? Mathf.Max(mouseRelativePos.x, 0f) :
             _Xlimit == LimitType.Max ? Mathf.Min(mouseRelativePos.x, 0f) :
             mouseRelativePos.x,
             _Ylimit == LimitType.Min ? Mathf.Max(mouseRelativePos.y, 0f) :
             _Ylimit == LimitType.Max ? Mathf.Min(mouseRelativePos.y, 0f) :
             mouseRelativePos.y,
            0f
        );

        Vector3 newPos = _wireOrigin.position + mouseRelativePos.normalized * Mathf.Min(mouseRelativePos.magnitude, lenght);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        _lineRenderer.SetPosition(1, transform.position);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, (new Vector3(mouseRelativePos.y, -mouseRelativePos.x, mouseRelativePos.z)));
    }

    public void OnMouseUp()
    {
        _isSelected = false;

        MoveToMousePos(_lenghtAtRest);
    }

    public void OnMouseDown()
    {
        _isSelected = true;
    }
}


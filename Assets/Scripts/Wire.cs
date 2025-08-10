using UnityEngine;

public class Wire : MonoBehaviour, IActivable
{
    [SerializeField] public Transform _wireOrigin;
    [SerializeField] private float _maxLength;
    [SerializeField] private float _lenghtAtRest;
    [SerializeField] private LimitType _Xlimit;
    [SerializeField] private LimitType _Ylimit;
    private LineRenderer _lineRenderer;
    private bool _isSelected;
    private Camera _camera;
    private Assemblable _assemblable => GetComponent<Assemblable>();
    private bool _isSoundOn;

    [SerializeField] private Sounds _uniqueSounds;

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

    public void OnConnectionsUpdate()
    {
        _isSoundOn = false;
        foreach (var collision in _assemblable.CurrentCollidingObjects)
        {
            if (collision.GetComponent<Assemblable>().StatusEffectAffinities[StatusEffect.Electrified] == StatusEffectAffinity.Conductor)
                _isSoundOn = true;
        }
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

        _uniqueSounds.PlaySoundContainer("PutDown");
        MoveToMousePos(_lenghtAtRest);
    }

    public void OnMouseDown()
    {
        _isSelected = true;
        _uniqueSounds.PlaySoundContainer("PickUp");
    }

    public void SetActive(bool value)
    {
        if (!value) return;
        _lineRenderer.SetPosition(0, new Vector3(_wireOrigin.position.x, _wireOrigin.position.y, transform.position.z));
        Vector3 wirePos = _wireOrigin.position;
        _wireOrigin.position = new Vector3(wirePos.x, wirePos.y, transform.position.z);
        _uniqueSounds.PlayLoopingContainer("Spark");

    }
}


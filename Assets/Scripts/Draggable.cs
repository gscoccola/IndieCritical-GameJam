using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// This script allows a GameObject to be dragged around the screen with the mouse.
// It exposes a public boolean field `IsSelected` to indicate whether the object is currently selected.

public class Draggable : MonoBehaviour
{
    public bool IsSelectable = true;
    public bool IsSelected { get; private set; }
    public bool IsFixed;
    public bool IsScrewed;

    [SerializeField] private bool _freezeX;
    [SerializeField] private bool _freezeY;
    [SerializeField] private bool _irreversible;
    [SerializeField] private LimitType _Xlimit;
    [SerializeField] private LimitType _Ylimit;
    //[SerializeField] private bool _centerOnMouse;

    private Camera _camera;
    private Vector3 _initialRelativePos;
    private bool _isVibrating;
    private Vector3 _initialPos;
    private Transform _parent;

    public UnityEvent OnSelected;
    public UnityEvent OnDeSelected;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        _parent = transform.parent;
        _initialPos = _parent.position;
        IsSelectable = true;
    }

    private void OnMouseDrag()
    {
        if (!IsSelected || IsFixed || !IsSelectable) return;
        
        Vector3 newPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        newPos += _initialRelativePos;
        /*if (!_centerOnMouse) newPos += _initialRelativePos;
        /*else newPos += GetComponent<Collider2D>().offset.x * Vector3.right;*/

        if (_irreversible) _initialPos = _parent.position;

        float newX = _Xlimit == LimitType.Min ? Mathf.Max(newPos.x, _initialPos.x) :
                     _Xlimit == LimitType.Max ? Mathf.Min(newPos.x, _initialPos.x) :
                     newPos.x;

        float newY = _Ylimit == LimitType.Min ? Mathf.Max(newPos.y, _initialPos.y) :
                     _Ylimit == LimitType.Max ? Mathf.Min(newPos.y, _initialPos.y) :
                     newPos.y;

        _parent.position = new Vector3(
            _freezeX ? _parent.position.x : newX,
            _freezeY ? _parent.position.y : newY,
            _parent.position.z
        );
    }

    private void OnMouseUp()
    {
        OnMouseAction(false);
        if (!IsFixed) OnDeSelected?.Invoke();
    }

    private void OnMouseDown()
    {
        if (!IsSelectable) return;
        if (IsScrewed)
        {
            StartCoroutine(IVibrate());
            return;
        }
        if (!IsFixed) OnSelected?.Invoke();
        OnMouseAction(true);
        _initialRelativePos = _parent.position - _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseAction(bool mouseDown)
    {
        IsSelected = mouseDown;
    }

    private IEnumerator IVibrate()
    {
        _isVibrating = true;
        yield return new WaitForSeconds(0.3f);
        _parent.position = _initialPos;
        _isVibrating = false;
    }

    private void Update()
    {
        if (_isVibrating) Vibrate();
    }

    private void Vibrate()
    {
        Vector3 vibration = Vector3.zero;
        if (_freezeX) vibration.y = Mathf.PingPong(Time.time * 3f, 0.05f);
        if (_freezeY) vibration.x = Mathf.PingPong(Time.time * 3f, 0.05f);
        _parent.position = _initialPos + vibration;
    }
}

public enum LimitType
{
    None,
    Min,
    Max,
}

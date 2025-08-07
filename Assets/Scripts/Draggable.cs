using System.Collections;
using UnityEngine;

// This script allows a GameObject to be dragged around the screen with the mouse.
// It exposes a public boolean field `IsSelected` to indicate whether the object is currently selected.

public class Draggable : MonoBehaviour
{
    public bool IsSelected { get; private set; }
    public bool isFixed;
    public bool isScrewed;

    [SerializeField] private bool _showIfSelected = true;
    [SerializeField] private bool _freezeX;
    [SerializeField] private bool _freezeY;
    [SerializeField] private LimitType _Xlimit;
    [SerializeField] private LimitType _Ylimit;

    private Camera _camera;
    private Vector3 _initialRelativePos;
    private bool _isVibrating;
    private Vector3 _initialPos;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        _initialPos = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!IsSelected || isFixed) return;
        
        Vector3 newPos = _camera.ScreenToWorldPoint(Input.mousePosition) + _initialRelativePos;

        float newX = _Xlimit == LimitType.Min ? Mathf.Max(newPos.x, _initialPos.x) :
                     _Xlimit == LimitType.Max ? Mathf.Min(newPos.x, _initialPos.x) :
                     newPos.x;

        float newY = _Ylimit == LimitType.Min ? Mathf.Max(newPos.y, _initialPos.y) :
                     _Ylimit == LimitType.Max ? Mathf.Min(newPos.y, _initialPos.y) :
                     newPos.y;

        transform.position = new Vector3(
            _freezeX ? transform.position.x : newX,
            _freezeY ? transform.position.y : newY,
            transform.position.z
        );
    }

    private void OnMouseUp()
    {
        OnMouseAction(false);
    }

    private void OnMouseDown()
    {
        if (isScrewed)
        {
            StartCoroutine(IVibrate());
            return;
        }
        OnMouseAction(true);
        _initialRelativePos = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseAction(bool mouseDown)
    {
        IsSelected = mouseDown;
        if (_showIfSelected) GetComponent<SpriteRenderer>().color = IsSelected ? new Color (0.8f, 0.8f, 0.8f , 0.8f) : Color.white;
    }

    private IEnumerator IVibrate()
    {
        _isVibrating = true;
        yield return new WaitForSeconds(0.3f);
        transform.position = _initialPos;
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
        transform.position = _initialPos + vibration;
    }
}

public enum LimitType
{
    None,
    Min,
    Max,
}

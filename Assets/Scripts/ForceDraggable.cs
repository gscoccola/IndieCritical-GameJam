using UnityEngine;
using UnityEngine.Events;

public class ForceDraggable : MonoBehaviour
{
    public bool IsSelectable = true;
    public bool IsSelected;
    public bool IsFixed;

    [SerializeField] private float _force = 5f;

    private Camera _camera;
    private Rigidbody2D _rb;

    public UnityEvent OnSelected;
    public UnityEvent OnDeSelected;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        IsSelectable = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsSelected || IsFixed || !IsSelectable) return;

        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _rb.AddForce(_force * Time.deltaTime * new Vector2(
            mousePos.x,
            mousePos.y
        ),
            ForceMode2D.Impulse);
    }

    private void OnMouseUp()
    {
        OnMouseAction(false);
        if (!IsFixed) OnDeSelected?.Invoke();
    }

    private void OnMouseDown()
    {
        if (!IsSelectable) return;
        OnSelected?.Invoke();
        OnMouseAction(true);
    }

    private void OnMouseAction(bool mouseDown)
    {
        IsSelected = mouseDown;
    }
}

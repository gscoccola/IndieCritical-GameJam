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

    private void FixedUpdate()
    {
        if (!IsSelected || IsFixed || !IsSelectable) return;

        Vector3 newPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        newPos = new Vector3(Mathf.Clamp(newPos.x, -10f, 10f), Mathf.Clamp(newPos.y, -5.625f, 5.625f), newPos.z);

        Vector3 mousePos = newPos - transform.position;
        _rb.AddForce(_force * Time.fixedDeltaTime * new Vector2(
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

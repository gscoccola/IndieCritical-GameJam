using UnityEngine;

// This script allows a GameObject to be dragged around the screen with the mouse.
// It exposes a public boolean field `IsSelected` to indicate whether the object is currently selected.

public class Draggable : MonoBehaviour
{
    public bool IsSelected;
    [SerializeField] private bool _showIfSelected = true;

    private Camera _camera;
    private Vector3 _initialRelativePos;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
    }

    private void OnMouseDrag()
    {
        if (!IsSelected) return;
        
        Vector3 newPos = _camera.ScreenToWorldPoint(Input.mousePosition) + _initialRelativePos;

        transform.position = new Vector3(
            newPos.x,
            newPos.y,
            0f
        );
    }

    private void OnMouseUp()
    {
        OnMouseAction(false);
    }

    private void OnMouseDown()
    {
        OnMouseAction(true);
        _initialRelativePos = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseAction(bool mouseDown)
    {
        IsSelected = mouseDown;
        if (_showIfSelected) GetComponent<SpriteRenderer>().color = IsSelected ? new Color (0.8f, 0.8f, 0.8f , 0.8f) : Color.white;
    }

}

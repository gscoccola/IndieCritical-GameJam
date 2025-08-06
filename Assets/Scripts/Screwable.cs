using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class Screwable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float _timeToUnscrew = 3f;

    public bool IsFree;

    private bool _isPressed;
    private float _timeLeft;
    private Rigidbody2D _rigidbody;
    private Draggable _draggable;

    private void Start()
    {
        _timeLeft = _timeToUnscrew;
        _rigidbody = GetComponent<Rigidbody2D>();
        _draggable = GetComponent<Draggable>();
        _draggable.isFixed = true;
    }

   private void Update()
    {
        if (IsFree) return;

        if (_isPressed)
        {
            _rigidbody.angularVelocity = rotationSpeed;
            _timeLeft -= Time.deltaTime;
        }
        else _rigidbody.angularVelocity = 0f;


        if (_timeLeft <= 0f)
        {
            IsFree = true;
            _draggable.isFixed = false;
            _rigidbody.angularVelocity = 0f;
        }
            
    }

    private void OnMouseUp()
    {
        if (IsFree) return;
        _isPressed = false;
    }

    private void OnMouseDown()
    {
        if (IsFree) return;
        _isPressed = true;
    }

 
}

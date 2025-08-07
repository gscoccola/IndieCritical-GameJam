using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class Screwable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float _timeToUnscrew = 3f;
    [SerializeField] ToolType _requiredType;

    public bool IsFree;

    private bool _isPressed;
    private float _timeLeft;
    private Rigidbody2D _rigidbody;
    private Draggable _draggable;
    private bool _isVibrating;

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

        if (_isVibrating)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * 100f, 10f));
            return;
        }

        if (_isPressed && ToolsManager.Instance.Tools[ToolType.Screwdriver] == true)
        {
            _rigidbody.angularVelocity = rotationSpeed;
            _timeLeft -= Time.deltaTime;
        }
        else _rigidbody.angularVelocity = 0f;


        if (_timeLeft <= 0f)
        {
            IsFree = true;
            gameObject.layer = LayerMask.NameToLayer("Assemblable");
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
        if (!ToolsManager.Instance.Tools[_requiredType])
        {
            StartCoroutine(IVibrate());
            return;
        }
        _isPressed = true;
    }

    private IEnumerator IVibrate()
    {
        _isVibrating = true;
        yield return new WaitForSeconds(0.3f);
        _isVibrating = false;
    }

}

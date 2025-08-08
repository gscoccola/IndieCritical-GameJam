using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Draggable))]
public class Screwable : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float _timeToUnscrew = 3f;
    [SerializeField] ToolType _requiredType;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] LayerMask _requiredToolLayer;

    public bool IsFree;

    private bool _isPressed;
    private float _timeLeft;
    private Rigidbody2D _rigidbody;
    private Draggable _draggable;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider => GetComponent<Collider2D>();
    private bool _isVibrating;
    public bool _isRolling;
    private float _rollTime;

    private void Start()
    {
        _timeLeft = _timeToUnscrew;
        _rigidbody = GetComponent<Rigidbody2D>();
        _draggable = GetComponent<Draggable>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _draggable.IsFixed = true;
    }

   private void Update()
    {
        if (_isRolling)
        {
            _rollTime += Time.deltaTime;
            _rigidbody.angularVelocity = 60f - Mathf.PingPong(_rollTime * 50f, 90f);
        }
            
        else _rigidbody.angularVelocity = 0f;

        if (IsFree) return;

        _isPressed = _collider.IsTouchingLayers(_requiredToolLayer);

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
            _draggable.IsFixed = false;
            _rigidbody.angularVelocity = 0f;
        }    
    }

    private void OnMouseDown()
    {
        StartCoroutine(IVibrate());
    }

    private IEnumerator IVibrate()
    {
        _isVibrating = true;
        yield return new WaitForSeconds(0.3f);
        _isVibrating = false;
    }

    public void StartRoll() => StartCoroutine(IRoll());

    private IEnumerator IRoll()
    {
        _isRolling = true;
        _rollTime = 0f;
        _spriteRenderer.sprite = _sprites[1];
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        GetComponent<CircleCollider2D>().offset = new Vector2(-0.71f, 0f);
        yield return new WaitForSeconds(2.3f);
        _isRolling = false;
    }

    public void SwitchToUpright()
    {
        _spriteRenderer.sprite = _sprites[0];
        _isRolling = false;
        GetComponent<CircleCollider2D>().offset = Vector2.zero;
    }
}

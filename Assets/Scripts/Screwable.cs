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
    public bool IsLoose;

    private bool _isPressed;
    private float _timeLeft;
    private Rigidbody2D _rigidbody;
    private Draggable _draggable;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider => GetComponent<Collider2D>();
    private bool _isVibrating;
    public bool _isRolling;
    private float _rollTime;
    private Tool _currentTool;

    public UnityEvent OnBecomePressed;
    public UnityEvent OnBecomeUnPressed;
    public UnityEvent OnBecomeFree;
    public UnityEvent OnStartRolling;


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

        if (IsFree) 
        {
            if (_currentTool != null)
            {
                _currentTool.SetActive(false);
                _currentTool = null;
            }   
            return; 
        }

        if (_isVibrating)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * 100f, 10f));
            return;
        }

        if (_isPressed)
        {
            _rigidbody.angularVelocity = rotationSpeed;
            _timeLeft -= Time.deltaTime;
        }
        else _rigidbody.angularVelocity = 0f;


        if (_timeLeft <= 0f)
        {
            IsFree = true;
            OnBecomeFree?.Invoke();
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
        IsLoose = true;
        gameObject.layer = LayerMask.NameToLayer("Assemblable");
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
        IsLoose = true;
        gameObject.layer = LayerMask.NameToLayer("Assemblable");
        GetComponent<CircleCollider2D>().offset = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ActiveSDHead"))
        {
            _isPressed = true;
            OnBecomePressed?.Invoke();
            _currentTool = col.transform.parent.GetComponent<Tool>();
            col.transform.parent.GetComponent<Tool>().SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ActiveSDHead"))
        {
            _isPressed = false;
            OnBecomeUnPressed?.Invoke();
            _currentTool = null;
            col.transform.parent.GetComponent<Tool>().SetActive(false);
        }
    }
}

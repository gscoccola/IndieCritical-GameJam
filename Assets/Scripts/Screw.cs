using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Screw : MonoBehaviour, IStateLoader
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float _timeToUnscrew = 1.5f;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] LayerMask _requiredToolLayer;
    [SerializeField] BoxCollider2D _sideCollider;
    [SerializeField] Vector3 _checkpointStartPos;
    private Draggable _draggable;

    public bool IsFree;
    public bool IsLoose;

    private bool _isPressed;
    private float _timeLeft;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private bool _isVibrating;
    public bool _isRolling;
    private float _rollTime;
    private Screwdriver _currentTool;

    public UnityEvent OnBecomePressed;
    public UnityEvent OnBecomeUnPressed;
    public UnityEvent OnBecomeFree;
    public UnityEvent OnStartRolling;


    private void Start()
    {
        
        _timeLeft = _timeToUnscrew;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _draggable = GetComponent<Draggable>();
        //_draggable.IsFixed = true;
        if (_isRolling) StartRoll();
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
            _rigidbody.angularVelocity = 200f - Mathf.PingPong(Time.time * 4000f, 400f);
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

    
    public void Vibrate()
    {
        StartCoroutine(IVibrate());
    }

    private IEnumerator IVibrate()
    {
        _isVibrating = true;
        yield return new WaitForSeconds(0.5f);
        _isVibrating = false;
    }

    public void StartRoll() => StartCoroutine(IRoll());

    private IEnumerator IRoll()
    {
        OnStartRolling?.Invoke();
        IsLoose = true;
        gameObject.layer = LayerMask.NameToLayer("Assemblable");
        _isRolling = true;
        _rollTime = 0f;
        _spriteRenderer.sprite = _sprites[1];
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        //GetComponent<CircleCollider2D>().offset = new Vector2(-0.71f, 0f);
        _sideCollider.enabled = true;
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
        _sideCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & _requiredToolLayer) != 0)
        {
            _isPressed = true;
            _currentTool = col.transform.parent.GetComponent<Screwdriver>();
            col.transform.parent.GetComponent<Screwdriver>().SetActive(true);

            if (IsLoose) return; 
            OnBecomePressed?.Invoke(); 


        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("PhillipsScrewHead"))
        {
            Vibrate();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & _requiredToolLayer) != 0)
        {
            _isPressed = false;
            OnBecomeUnPressed?.Invoke();
            if (_currentTool != null) _currentTool.SetActive(false);
            _currentTool = null;
            col.transform.parent.GetComponent<Screwdriver>().SetActive(false);
        }
    }

    public void LoadState()
    {
        transform.position = _checkpointStartPos;
        IsFree = true;
        GetComponent<Draggable>().IsFixed = false;
        GetComponent<Assemblable>().SetActive(true);
        IsLoose = true;
        gameObject.layer = LayerMask.NameToLayer("Assemblable");
        GetComponent<SpriteRenderer>().sprite = _sprites[1];
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}

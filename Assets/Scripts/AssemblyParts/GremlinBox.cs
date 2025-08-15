using System.Collections;
using UnityEngine;

public class GremlinBox : MonoBehaviour
{
    [SerializeField] Gremlin _Gremlin;

    [SerializeField] private Transform _cover;
    [SerializeField] private Transform _background;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Collider2D _musicCollider;
    [SerializeField] Target _target;
    [SerializeField] float _handOffset;
    private Draggable _coverDraggable;
    private float _displacement;
    private float _timeToReturn;
    private Animator _animator;


    private float _trustTimer;
    private float _trustTime = 3f;

    private float _newRelDis;
    
    public bool _hasMusic;

    [SerializeField] float _autoReturnTime = 1.5f;
    private float _autoReturnTimer;
    [SerializeField] float _maxDisplacement = 0.5f;
    [SerializeField] private float _returnSpeed = 0.5f;
    private BoxState _boxState;

    private void Start()
    {
        _animator = _cover.GetComponent<Animator>();
        _timeToReturn = _maxDisplacement / _returnSpeed;
        _boxState = BoxState.Closed;
        _coverDraggable = _cover.GetComponent<Draggable>();
    }

    private void Update()
    {
        _hasMusic = _musicCollider.IsTouchingLayers(_layerMask);
        _displacement = _background.position.y -_cover.position.y + 0.103f;
        _autoReturnTimer -= Time.deltaTime;
        _trustTimer -= Time.deltaTime;
        //Debug.Log(_trustTimer);
        //Debug.Log(_boxState);

        if (_boxState == BoxState.Open)
        {
            _newRelDis = Mathf.MoveTowards(_newRelDis, 1.3f, Time.deltaTime * _returnSpeed);
            _cover.position = new Vector3(_cover.position.x, transform.position.y - _newRelDis + 0.103f, _cover.position.z);
        }

        if (_boxState == BoxState.HeadOut && !_hasMusic)
        {
            _boxState = BoxState.HandOut;
            _target.gameObject.SetActive(false);
            StartCoroutine(IHandOut());
        }

        if (_boxState == BoxState.Returning)
        {
            _newRelDis = Mathf.MoveTowards(_newRelDis, 0, Time.deltaTime * _returnSpeed);
            _cover.position = new Vector3(_cover.position.x, transform.position.y - _newRelDis + 0.103f, _cover.position.z);
            if (_displacement < 0f)
            {
                _boxState = BoxState.Closed;
                _coverDraggable.IsSelectable = true;
                StopAllCoroutines();
            }
            return;
        }

        if (_boxState == BoxState.Opening)
        {
            _coverDraggable.IsSelectable = false;
            _newRelDis = Mathf.MoveTowards(_newRelDis, _maxDisplacement *2f, Time.deltaTime * _returnSpeed);
            _cover.position = new Vector3(_cover.position.x, transform.position.y - _newRelDis + 0.103f, _cover.position.z);
            if (_displacement > _maxDisplacement * 1.5f - 0.05f)
            {
                _animator.SetBool("IsHandIn", false);
                _boxState = BoxState.HeadOut;
                _target.gameObject.SetActive(true);
                _coverDraggable.IsSelectable = true;
                //StartCoroutine(IHeadOut());
            }
            return;
        }

        if (_boxState == BoxState.Closed && _displacement > 0.1f)
        {
            _boxState = BoxState.Ajar;
            _autoReturnTimer = _autoReturnTime;
            return;
        }

        if ((_boxState == BoxState.Closed || _boxState == BoxState.Returning) && _hasMusic && _trustTimer < 0f)
        {
            _boxState = BoxState.Opening;
            _animator.SetBool("IsHandIn", true);
        }

        if (_boxState == BoxState.Ajar && _displacement > _maxDisplacement )
        {
            _boxState = BoxState.HandOut;
            StartCoroutine(IHandOut());
            return;
        }
        else if (_boxState == BoxState.Ajar && _autoReturnTimer < 0f)
        {
            _boxState = BoxState.Returning;
            StartCoroutine(ISlideBack());
        }

    }

    private IEnumerator ISlideBack()
    {
        _newRelDis = _background.position.y - _cover.position.y;
        
        _boxState = BoxState.Returning;
        yield return new WaitForSeconds(_timeToReturn * 0.5f);
        _animator.SetBool("IsHandIn", false);
        /*yield return new WaitForSeconds(_returnTime);
        _boxState = BoxState.Closed;
        _coverDraggable.IsSelectable = true;*/
    }

    private IEnumerator IHandOut()
    {
        _coverDraggable.IsSelectable = false;
        _animator.SetBool("IsHandIn", true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ISlideBack());
    }

    public void HeadHide()
    {
        if (_boxState != BoxState.HeadOut) return;
        _coverDraggable.IsSelectable = false;
        _target.gameObject.SetActive(false);
        StartCoroutine(IHeadHide());
    }

    private IEnumerator IHeadHide()
    {
        yield return new WaitForSeconds(0.2f);
        _boxState = BoxState.HandOut;
        _trustTimer = _trustTime;
        StartCoroutine(IHandOut());
    }


    public void OnHit()
    {
        _boxState = BoxState.Open;
        _target.gameObject.SetActive(false);
        _Gremlin.SetActive();
    }

}

public enum BoxState
{
    Closed,
    Ajar,
    HandOut,
    HeadOut,
    Returning,
    Opening,
    Open,
    Cooldown,
}
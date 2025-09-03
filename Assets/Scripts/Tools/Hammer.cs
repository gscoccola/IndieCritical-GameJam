using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hammer : MonoBehaviour
{
    [SerializeField] private GameObject _toolHead;

    public UnityEvent OnStartActive;
    public UnityEvent OnHit;

    public bool IsActive;
    public Target _currentTarget;

    private Animator _animator => GetComponent<Animator>();
    //private bool _hitAvailable = true;
    private float _hitCooldown = 1f;
    private float _hitTimer;

    public void Ready()
    {
        _toolHead.SetActive(true);
    }

    public void UnReady()
    {
        _toolHead.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        if ( !isActive)
        {
            StopAllCoroutines();
            _animator.SetTrigger("Stop");
            _animator.ResetTrigger("Hit");
            return;
        }
        
    }

    private void Update()
    {
        if (IsActive && _hitTimer <= 0.0f)
        {
            StartCoroutine(IHit()); 
        }
        _hitTimer -= Time.deltaTime;
    }

    private IEnumerator IHit()
    {
        _animator.SetTrigger("Hit");
        _animator.ResetTrigger("Stop");
        
        OnStartActive?.Invoke();
        yield return new WaitForSeconds(1.0f);
        OnHit?.Invoke();
        _currentTarget.OnHit?.Invoke();
        yield return new WaitForSeconds(0.5f);
        _animator.SetTrigger("Stop");
        _animator.ResetTrigger("Hit");
        _hitTimer = _hitCooldown;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hammer : MonoBehaviour
{
    [SerializeField] private GameObject _toolHead;

    public UnityEvent OnStartActive;
    public UnityEvent OnHit;

    public bool IsActive;

    private Animator Animator => GetComponent<Animator>();
    //private bool _hitAvailable = true;
    private float _hitCooldown = 2.5f;
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
        Animator.SetTrigger("Hit");
        _hitTimer = _hitCooldown;
        OnStartActive?.Invoke();
        yield return new WaitForSeconds(1.5f);
        OnHit?.Invoke();
    }
}

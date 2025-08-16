using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Screwdriver : MonoBehaviour
{
    [SerializeField] private GameObject _toolHead;

    public UnityEvent OnStartActive;
    public UnityEvent OnStopActive;

    public bool IsActive;

    private Animator Animator => GetComponent<Animator>();


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
        Animator.SetBool("IsActive", isActive);

        if (isActive) OnStartActive?.Invoke();
        else OnStopActive?.Invoke();

    }

    public void SwitchHead()
    {
        Animator.SetTrigger("Eaten");
        _toolHead.gameObject.layer = LayerMask.NameToLayer("FlatScrewHead");
        _toolHead.transform.localPosition -= 0.39f * Vector3.right;
    }


}

public enum ToolType
{
    Scredriver,
}
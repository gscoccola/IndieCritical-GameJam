using Unity.VisualScripting;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private LayerMask _toolTypeMask;
    [SerializeField] private GameObject _toolHead;

    public bool IsActive;

    private Animator Animator => GetComponent<Animator>();

    private void OnMouseDown()
    {
        _toolHead.SetActive(true);
    }

    private void OnMouseUp()
    {
        _toolHead.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Animator.SetBool("IsActive", isActive);
    }
}

public enum ToolType
{
    Scredriver,
}
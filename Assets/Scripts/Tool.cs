using Unity.VisualScripting;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private LayerMask _toolTypeMask;
    [SerializeField] private GameObject _toolHead;

    public bool IsActive;

    private Animator Animator => GetComponent<Animator>();

    /*private void OnMouseDown()
    {
        ToolsManager.Instance.PickupTool(type);
        Destroy(gameObject, 0.2f);
    }*/

    private void Start()
    {
        /*LayerMask.
        Debug.Log(LayerMask.NameToLayer("ActiveSDHead"));
        Debug.Log(_toolTypeMask.value);*/
    }

    private void OnMouseDown()
    {
        _toolHead.gameObject.layer = LayerMask.NameToLayer("ActiveSDHead");
    }

    private void OnMouseUp()
    {
        _toolHead.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Animator.SetBool("IsActive", isActive);
    }
}

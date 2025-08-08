using Unity.VisualScripting;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private LayerMask _toolTypeMask;
    [SerializeField] private GameObject _toolHead;

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
        Debug.Log(_toolTypeMask.value.ToBinaryString());
        _toolHead.gameObject.layer = LayerMask.NameToLayer("ActiveSDHead");
    }

    private void OnMouseUp()
    {
        _toolHead.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}

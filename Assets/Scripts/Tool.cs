using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private ToolType type;

    private void OnMouseDown()
    {
        ToolsManager.Instance.PickupTool(type);
        Destroy(gameObject, 0.2f);
    }
}

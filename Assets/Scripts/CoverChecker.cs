using UnityEngine;

public class CoverChecker : MonoBehaviour
{
    private Assemblable _parentAssemblable => transform.parent.GetComponent<Assemblable>();
    public bool IsCovered;

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Cover") /*|| col.transform.position.z > transform.position.z*/) return;
        if ((int)_parentAssemblable._heightLayer - (int)col.GetComponent<Assemblable>()._heightLayer != 1) return;
        _parentAssemblable.SetActive(true);
        Destroy(this.gameObject, 0.1f);
    }

    /*private void Update()
    {
        IsCovered = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Cover"));
    }*/
}

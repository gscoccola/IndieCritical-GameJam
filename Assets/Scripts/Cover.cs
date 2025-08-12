using UnityEngine;

public class Cover : MonoBehaviour
{
    private Draggable _draggable;
    //private Assemblable _assemblable;
    private Collider2D _collider => GetComponent<Collider2D>();

    private void Start()
    {
        //_assemblable = GetComponent<Assemblable>();
        _draggable = GetComponent<Draggable>();
    }

    private void Update()
    {
        CheckIfLocked();
    }

    public void CheckIfLocked()
    {
        _draggable.IsScrewed = _collider.IsTouchingLayers(LayerMask.GetMask("Screw")) 
            ?  true 
            :  false;
    }
}

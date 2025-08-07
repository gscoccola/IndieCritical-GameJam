using UnityEngine;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(Assemblable))]
public class Cover : MonoBehaviour
{
    private Draggable _draggable;
    private Assemblable _assemblable;

    private void Start()
    {
        _assemblable = GetComponent<Assemblable>();
        _draggable = GetComponent<Draggable>();
    }

    public void CheckIfLocked()
    {
        if (_assemblable.CurrentCollisions.Count == 0)
        {
            _draggable.isScrewed = false;
        }

    }
}

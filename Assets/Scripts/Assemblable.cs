using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Assemblable : MonoBehaviour
{
    public List<GameObject> CurrentCollisions = new List<GameObject>();
    public UnityEvent  OnCollisionUpdate;

    private void Start()
    {
        InitialCheck();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Assemblable>() == null) return;
        CurrentCollisions.Add(col.gameObject);
        Assembly.Instance.UpdateConnections(true, this, col.GetComponent<Assemblable>());
        OnCollisionUpdate?.Invoke();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Assemblable>() == null) return;
        CurrentCollisions.Remove(col.gameObject);
        Assembly.Instance.UpdateConnections(false, this, col.GetComponent<Assemblable>());
        OnCollisionUpdate?.Invoke();
    }

    private void InitialCheck()
    {
        Collider2D[] colliders = new Collider2D[10];
        GetComponent<Collider2D>().Overlap(new ContactFilter2D(), colliders);

        foreach (var col in colliders)
        {
            if (col != null && col.GetComponent<Assemblable>() != null)
            {
                CurrentCollisions.Add(col.gameObject);
                Assembly.Instance.UpdateConnections(true, this, col.GetComponent<Assemblable>());
            }
        }
    }
}

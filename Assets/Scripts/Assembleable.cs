using System.Collections.Generic;
using UnityEngine;

public class Assembleable : MonoBehaviour
{
    public List<GameObject> currentCollisions = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        currentCollisions.Add(col.gameObject);
        Assembly.Instance.UpdateConnections(true, this, col.GetComponent<Assembleable>());
    }

    void OnTriggerExit2D(Collider2D col)
    {
        currentCollisions.Remove(col.gameObject);
        Assembly.Instance.UpdateConnections(false, this, col.GetComponent<Assembleable>());
    }
}

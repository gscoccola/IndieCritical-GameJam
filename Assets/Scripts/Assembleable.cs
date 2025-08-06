using System.Collections.Generic;
using UnityEngine;

public class Assembleable : MonoBehaviour
{
    public List<GameObject> currentCollisions = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        currentCollisions.Add(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        currentCollisions.Remove(col.gameObject);
    }
}

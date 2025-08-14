using UnityEngine;

public class Mouth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Screw>() != null)
        {
            Destroy(col.gameObject, 0.2f);
        }

    }
}

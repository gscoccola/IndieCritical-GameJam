using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    private Centipede _centipede;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Screw>() != null)
        {
            col.transform.position += 1000f * Vector3.up;
            col.GetComponent<Draggable>().IsSelectable = false;
            StartCoroutine(IGrow());
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("PhillipsScrewHead"))
        {
            col.gameObject.GetComponentInParent<Screwdriver>().SwitchHead();
            StartCoroutine(IGrow());
            StartCoroutine(IGrow());
        }

    }

    private IEnumerator IGrow()
    {
        yield return new WaitForSeconds(0.2f);
        _centipede.AddSegment();
        /*yield return new WaitForSeconds(0.2f);
        _centipede.AddSegment();*/
    }
}

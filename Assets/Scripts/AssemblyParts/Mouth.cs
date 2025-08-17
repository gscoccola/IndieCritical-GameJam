using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    private Centipede _centipede;
    private Sounds _sounds;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
        _sounds = GetComponentInParent<Sounds>();
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
            col.gameObject.GetComponentInParent<Draggable>().IsSelected = false;
            StartCoroutine(IGrow(2));
        }

    }

    private IEnumerator IGrow(int number = 1)
    {
        _sounds.PlaySoundContainer("Eat");

        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(0.4f);
            _centipede.AddSegment();
        }

    }
}

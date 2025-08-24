using UnityEngine;
using System.Collections;

public class Mouth : MonoBehaviour
{
    private Centipede _centipede;
    private Sounds _sounds;
    [SerializeField] GameObject _particles;
    [SerializeField] Rigidbody2D _rb;

    private void Start()
    {
        _centipede = GetComponentInParent<Centipede>();
        _sounds = GetComponentInParent<Sounds>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_rb.rotation > 190f)
        {
            _rb.AddTorque(-60f * Time.fixedDeltaTime);
        }
        else if (_rb.rotation < 170f)
        {
            _rb.AddTorque(60f * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Screw>() != null)
        {
            TurnHead(col);
            col.transform.position += 1000f * Vector3.up;
            col.GetComponent<Draggable>().IsSelectable = false;
            StartCoroutine(IGrow());
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("PhillipsScrewHead"))
        {
            col.gameObject.GetComponentInParent<Screwdriver>().SwitchHead();
            col.gameObject.GetComponentInParent<Draggable>().IsSelected = false;
            TurnHead(col);
            StartCoroutine(IGrow(2));
        }

    }

    private void TurnHead(Collider2D col)
    {
        if (col.transform.position.y > transform.position.y)
        {
            _rb.AddTorque(300f);
        }
        else
        {
            _rb.AddTorque(-300f);
        }
    }

    private IEnumerator IGrow(int number = 1)
    {
        _sounds.PlaySoundContainer("Eat");

        Instantiate(_particles, transform.position - 0.1f * Vector3.forward, Quaternion.identity);
        for (int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(0.4f);
            _centipede.AddSegment();
        }

    }
}

using System;
using UnityEngine;

public class VeinConnection : MonoBehaviour
{
    private LineRenderer _lineRenderer => GetComponent<LineRenderer>();
    private Transform _origin;
    private Transform _destination;
    private Vector3 _offset;

    public void Setup(Assemblable origin, Assemblable destination, bool randomize)
    {
        _origin = origin.transform;
        _destination = destination.transform;
        _offset = randomize ? UnityEngine.Random.insideUnitCircle : Vector3.zero;
    }

    public void Setup(Transform origin, Transform destination, bool randomize)
    {
        _origin = origin;
        _destination = destination;
        _offset = randomize ? UnityEngine.Random.insideUnitCircle * 0.8f : Vector3.zero;
    }

    private void Update()
    {
        if (_destination == null) 
        {
            Destroy(gameObject);
            return;
        }
        _lineRenderer.SetPosition(0, ShiftBelow(_origin.position.Flatten() + _offset));
        _lineRenderer.SetPosition(1, ShiftBelow(_destination.position.Flatten()));
    }

    private Vector3 ShiftBelow(Vector3 vector)
    {         return new Vector3(vector.x, vector.y, vector.z + 3.1f);
    }
}

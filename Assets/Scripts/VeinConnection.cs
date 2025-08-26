using System;
using UnityEngine;

public class VeinConnection : MonoBehaviour
{
    private LineRenderer _lineRenderer => GetComponent<LineRenderer>();
    private Assemblable _origin;
    private Assemblable _destination;

    public void Setup(Assemblable origin, Assemblable destination)
    {
        _origin = origin;
        _destination = destination;
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, ShiftBelow(_origin.transform.position.Flatten()));
        _lineRenderer.SetPosition(1, ShiftBelow(_destination.transform.position.Flatten()));
    }

    private Vector3 ShiftBelow(Vector3 vector)
    {         return new Vector3(vector.x, vector.y, vector.z + 3.1f);
    }
}

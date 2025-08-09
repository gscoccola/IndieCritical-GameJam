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
        _lineRenderer.SetPosition(0, _origin.transform.position);
        _lineRenderer.SetPosition(1, _destination.transform.position);
    }
}

using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField] GameObject _centipedeSegmentPrefab;
    [SerializeField] private GameObject _head;
    private GameObject _lastBodySegment;
    [SerializeField] private float _speed;
    private float _rotationSpeed;
    private float _segmentDistance = 0.33f;
    public bool AddSegmentTrigger = false;
    private int _length = 1;
    private bool _isOurobouros = false;
    private Vector3 _ourobourosCenter;

    private List<GameObject> _segments = new List<GameObject>();

    private void Start()
    {
        _rotationSpeed = - _speed;
        _lastBodySegment = _head;
        _segments.Add(_head);
        /*for (int i = 0; i < 10; i++)
        {
            AddSegment();
        }*/
    }

    private void Update()
    {
        if (AddSegmentTrigger)
        {
            AddSegment();
            AddSegmentTrigger = false;
        }

        if (!_isOurobouros && _length > 10 && _head.GetComponent<Collider2D>().IsTouching(_lastBodySegment.GetComponent<Collider2D>()))
        {
            TriggerOurobouros();
        }

        if (_isOurobouros)
        {
            foreach (var segment in _segments)
            {
                segment.GetComponent<Rigidbody2D>().AddForce(-Time.deltaTime * _rotationSpeed *
                    OrthogonalVector(segment.transform.position - _ourobourosCenter).normalized);

                segment.GetComponent<Rigidbody2D>().AddForce(-Time.deltaTime * _speed *
                        (segment.transform.position - _ourobourosCenter));

            }
        }
    }

    public void AddSegment()
    {
        GameObject newSegment = Instantiate(_centipedeSegmentPrefab,
            _lastBodySegment.transform.position.Flatten() + _lastBodySegment.transform.right * _segmentDistance - 0.05f * _length * _lastBodySegment.transform.forward
            , _lastBodySegment.transform.rotation, transform);
        newSegment.GetComponent<HingeJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();
        _lastBodySegment = newSegment;
        _segments.Add(newSegment);
        _length++;
    }

    private void TriggerOurobouros()
    {
        _head.GetComponent<Mouth>().enabled = false;
        _ourobourosCenter = AveragePosition(_segments); 
        if (_head.transform.position.y < _ourobourosCenter.y) _rotationSpeed *= -1f;
        foreach (var segment in _segments)
        {
            segment.GetComponent<ForceDraggable>().IsSelectable = false;
        }
        _isOurobouros = true;
        AddSegment();
        //_lastBodySegment.transform.rotation = _firstBodySegment.transform.rotation;
        //_lastBodySegment.transform.position = _firstBodySegment.transform.position;

        //_firstBodySegment.GetComponent<HingeJoint2D>().enabled = false;

        /*_firstBodySegment.GetComponent<HingeJoint2D>().useLimits = false;
        _firstBodySegment.GetComponent<HingeJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();*/

        _head.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _head.GetComponent<SpringJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();
        //_head.GetComponent<HingeJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();
    }

    private Vector3 AveragePosition(List<GameObject> objects)
    {
        Vector3 sum = Vector3.zero;
        foreach (var obj in objects)
        {
            sum += obj.transform.position;
        }
        return sum / objects.Count;
    }

    private Vector3 OrthogonalVector(Vector3 vector)
    {
        return new Vector3(-vector.y, vector.x, vector.z);
    }
}

public static class VectorExtensions
{
    public static Vector3 Orthogonal(this Vector3 vector)
    {
        return new Vector3(-vector.y, vector.x, vector.z);
    }

    public static Vector3 Flatten(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}

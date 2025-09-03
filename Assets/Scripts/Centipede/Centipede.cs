using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour, IStateLoader
{
    [SerializeField] GameObject _centipedeSegmentPrefab;
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _gpu;
    [SerializeField] private SpriteRenderer _headSprite;
    private GPU _gpuComponent;
    [SerializeField] private ParticleSystem _goreChunks;
    [SerializeField] private float _blenderForce;
    public GameObject _lastBodySegment;
    [SerializeField] private float _speed;
    private float _rotationSpeed;
    private float _segmentDistance = 0.33f;
    public bool AddSegmentTrigger = false;
    private int _length = 1;
    private bool _isOurobouros = false;
    private Vector3 _ourobourosCenter;
    private bool _eatingLastSegment;
    private bool _canOurobouros = true;
    [SerializeField] private ParticleSystem _bloodParticleSystem;
    [SerializeField] private GameObject _target;

    private List<GameObject> _segments = new List<GameObject>();
    private Sounds _sounds;

    private void Awake()
    {
        _rotationSpeed = _speed;
        _lastBodySegment = _head;
        _segments.Add(_head);
        _sounds = transform.GetComponent<Sounds>();
        _gpuComponent = _gpu.transform.GetChild(0).transform.GetComponent<GPU>();
        for (int i = 0; i < 8; i++)
        {
            AddSegment();
        }
    }

    private void Update()
    {
        if (AddSegmentTrigger)
        {
            AddSegment();
            AddSegmentTrigger = false;
        }

        if (_canOurobouros && !_isOurobouros && _length > 10 && _head.GetComponent<Collider2D>()
            .IsTouching(_lastBodySegment.GetComponent<Collider2D>()))
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

        else if (_eatingLastSegment)
        {
            if (_lastBodySegment == null) return;
            _lastBodySegment.GetComponent<Rigidbody2D>().AddForce(Time.deltaTime * _blenderForce *
                ((_gpu.transform.position - _lastBodySegment.transform.position).normalized));
        }
    }

    public void AddSegment()
    {
        float layerOffset = _lastBodySegment == _head ? 0.5f : -0.005f;
        GameObject newSegment = Instantiate(_centipedeSegmentPrefab,
            _lastBodySegment.transform.position + _lastBodySegment.transform.right * _segmentDistance 
            + _length * layerOffset * _lastBodySegment.transform.forward
            , _lastBodySegment.transform.rotation, transform);
        //_head.transform.position = new Vector3(_head.transform.position.x, _head.transform.position.y, -0.9f);
        newSegment.GetComponent<HingeJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();
        newSegment.GetComponent<VelocityVolumeController>().SetSounds(_sounds);
        newSegment.GetComponent<BodyPart>()._mouth = _head;
        _lastBodySegment = newSegment;
        _segments.Add(newSegment);
        _length++;
    }

    private void TriggerOurobouros()
    {
        SubobjectivesManager.Instance.TriggerEnding(Endings.Ourobouros);
        _head.GetComponent<Mouth>().enabled = false;
        _ourobourosCenter = AveragePosition(_segments); 
        if (_head.transform.position.y < _ourobourosCenter.y) _rotationSpeed *= -1f;
        foreach (var segment in _segments)
        {
            segment.GetComponent<ForceDraggable>().IsSelectable = false;
        }
        _isOurobouros = true;
        _sounds.PlayLoopingContainer("Orobourus_Movement");
        MusicPlayer.Instance.SetOrobourus();
        AddSegment();
        _head.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _head.GetComponent<SpringJoint2D>().enabled = true;
        _head.GetComponent<SpringJoint2D>().connectedBody = _lastBodySegment.GetComponent<Rigidbody2D>();
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

    public void TriggerBlender()
    {
        MusicPlayer.Instance.SetBlender();
        StartCoroutine(ITriggerBlender());
    }

    private IEnumerator ITriggerBlender()
    {
        _canOurobouros = false;
        _gpuComponent._isFarming = true;
        _bloodParticleSystem.Play();
        yield return new WaitForSeconds(0.1f);
        _eatingLastSegment = true;
        //_head.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _head.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        _lastBodySegment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        foreach (var segment in _segments)
        {
            //segment.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            segment.GetComponent<ForceDraggable>().IsSelectable = false;
            Destroy(GetComponent<VelocityVolumeController>());
        }
        yield return new WaitForSeconds(2f);
        _lastBodySegment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        while (true)
        {
            _blenderForce -= 80f;
            yield return new WaitForSeconds(1.5f);
            Destroy(_lastBodySegment);
            
            _segments.RemoveAt(_segments.Count - 1);
            _lastBodySegment = _segments[_segments.Count - 1];
            if (_segments.Count == 2)
            {
                _target.gameObject.SetActive(true);
                if (_gpuComponent.IsSpinning) MusicPlayer.Instance.StopBlender();
                _gpuComponent.IsSpinning = false;
                _gpuComponent._isFarming = false;
                _eatingLastSegment = false;
                _bloodParticleSystem.Stop();
                break;
            }
            
        } 
    }

    public void LastHit()
    {
        if (_eatingLastSegment) return;
        _blenderForce = 200f;
        _gpuComponent.StartSpinning();
        _gpuComponent._isFarming = true;
        _gpuComponent._bloodSpeed *= 2f;
        Destroy(_lastBodySegment);
        Destroy(_segments[_segments.Count - 1]);
        _lastBodySegment = _head;
        _eatingLastSegment = true;
        _bloodParticleSystem.Play();
        StartCoroutine(ILastBlood());
    }

    private IEnumerator ILastBlood()
    {
        yield return new WaitForSeconds(1.5f);
        _headSprite.enabled = false;
        _sounds.PlaySoundContainer("HeadExplode");
        _goreChunks.Play();
        if (_length > 7)
        {
            SubobjectivesManager.Instance.SetGPU(true);
            _gpuComponent._backAnimator.SetTrigger("PowerOn");
        }
        else SubobjectivesManager.Instance.TriggerEnding(Endings.Softlocked);
        yield return new WaitForSeconds(0.8f);
        _bloodParticleSystem.Stop();
        _gpuComponent._isFarming = false;


    }

    public void TriggerAbsorption()
    {
        foreach (var segment in _segments)
        {
            //segment.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            segment.GetComponent<ForceDraggable>().IsSelectable = false;
            Destroy(GetComponent<VelocityVolumeController>());
        }
    }

    public void LoadState()
    {
        AddSegment();
        AddSegment();
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

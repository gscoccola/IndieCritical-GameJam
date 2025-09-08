using UnityEngine;

public class Eye : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _initialPos;
    public bool IsStunned;
    public float _stunnedSpeed;
    [SerializeField] private float Bound = 0.015625f;

    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        _initialPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (IsStunned)
        {
            transform.localPosition = new Vector3(-Bound + Mathf.PingPong(_stunnedSpeed * Time.time + Mathf.PI, 2 * Bound),
                -Bound + Mathf.PingPong(_stunnedSpeed * Time.time + 2 * Mathf.PI, 2 * Bound),
                _initialPos.z);
            return;
        }
        
        Vector3 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.localPosition = new Vector3(-Mathf.Clamp(direction.x, -Bound, Bound),
            -Mathf.Clamp(direction.y, -Bound, Bound),
            _initialPos.z);

    }
}

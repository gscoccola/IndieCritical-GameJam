using UnityEngine;

public class GPU : MonoBehaviour
{
    private bool _hasBegun;
    [SerializeField] private Sprite _bloodySprite;
    [SerializeField] private Transform _bloodLevel;
    [SerializeField] private float _bloodSpeed = 0.2f;
    private SpriteRenderer _spriteRenderer;
    public bool IsSpinning;
    public bool _isFarming;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!IsSpinning) return;
        if (_isFarming)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * 200f);
            _bloodLevel.position += Time.deltaTime * _bloodSpeed * Vector3.right;
        }
        else transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * 800f);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsSpinning) return;
        if (!_hasBegun)
        {
            _spriteRenderer.sprite = _bloodySprite;
            _hasBegun = true;
            col.transform.parent.GetComponent<Centipede>().TriggerBlender();
        }
        
    }
}

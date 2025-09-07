using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 3f;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if ( _lifeTime < 0 )
            Destroy( gameObject );
    }
}

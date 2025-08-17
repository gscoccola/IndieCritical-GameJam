using UnityEngine;

public class GPU : MonoBehaviour
{
    private bool _hasBegun;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_hasBegun)
        {
            _hasBegun = true;
            col.transform.parent.GetComponent<Centipede>().TriggerBlender();
            gameObject.SetActive(false);
        }
        
    }
}

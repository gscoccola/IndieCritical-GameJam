using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    private Hammer _currentTool;
    public UnityEvent OnHit;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("HammerHead"))
        {
            _currentTool = col.transform.parent.GetComponent<Hammer>();
            _currentTool._currentTarget = this;
            col.transform.parent.GetComponent<Hammer>().SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("HammerHead"))
        {
            if (_currentTool != null) { _currentTool.SetActive(false); _currentTool._currentTarget = null; }
                _currentTool = null;
            col.transform.parent.GetComponent<Hammer>().SetActive(false);
        }
    }

   
}

using UnityEngine;

public class Target : MonoBehaviour
{
    private Hammer _currentTool;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("HammerHead"))
        {
            _currentTool = col.transform.parent.GetComponent<Hammer>();
            col.transform.parent.GetComponent<Hammer>().SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("HammerHead"))
        {
            if (_currentTool != null) _currentTool.SetActive(false);
            _currentTool = null;
            col.transform.parent.GetComponent<Hammer>().SetActive(false);
        }
    }
}

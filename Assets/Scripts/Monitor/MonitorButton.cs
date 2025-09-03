using UnityEngine;
using System.Collections;

public class MonitorButton : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    public bool canBePressed = true;

    private void OnMouseDown()
    {
        if (!canBePressed) return;
        canBePressed = false;
        StartCoroutine(IHandRoutine());
    }

    private IEnumerator IHandRoutine()
    {
        _handAnimator.SetTrigger("Hand");
        yield return new WaitForSeconds(4f);
        _handAnimator.ResetTrigger("Hand");
        canBePressed = true;
    }
}


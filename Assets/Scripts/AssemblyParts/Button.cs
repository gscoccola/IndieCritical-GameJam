using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] RandomSoundContainer _buttonPress;
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnActivate()
    {
        _animator.SetTrigger("Pressed");
        _buttonPress.PlayRandomSoundOnTarget(transform);
    }
}

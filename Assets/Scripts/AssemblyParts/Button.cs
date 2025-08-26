using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] RandomSoundContainer _buttonPress;
    Animator _animator;
    private GPU _gpu;
    private bool _pressed = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gpu = FindAnyObjectByType<GPU>();
    }

    public void OnActivate()
    {
        if (_pressed) return;
        _pressed = true;
        _gpu.StartSpinning();
        _animator.SetTrigger("Pressed");
        _buttonPress.PlayRandomSoundOnTarget(transform);

    }
}

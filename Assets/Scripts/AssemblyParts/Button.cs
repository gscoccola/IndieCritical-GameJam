using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] RandomSoundContainer _buttonPress;
    Animator _animator;
    private GPU _gpu;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gpu = FindAnyObjectByType<GPU>();
    }

    public void OnActivate()
    {
        _gpu.IsSpinning = true;
        _animator.SetTrigger("Pressed");
        _buttonPress.PlayRandomSoundOnTarget(transform);
    }
}

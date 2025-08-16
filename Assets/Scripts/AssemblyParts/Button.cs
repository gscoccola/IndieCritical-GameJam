using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Sprite _pressedSprite;
    [SerializeField] RandomSoundContainer _buttonPress;

    public void OnActivate()
    {
        GetComponent<SpriteRenderer>().sprite = _pressedSprite;
        _buttonPress.PlayRandomSoundOnTarget(transform);
    }
}

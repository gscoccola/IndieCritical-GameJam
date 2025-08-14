using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Sprite _pressedSprite;

    public void OnActivate()
    {
        GetComponent<SpriteRenderer>().sprite = _pressedSprite;
    }
}

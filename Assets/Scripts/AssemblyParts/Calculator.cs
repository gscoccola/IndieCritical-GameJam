using UnityEngine;

public class Calculator : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    public void SetPowered(bool value)
    {
        GetComponent<SpriteRenderer>().sprite = value ? _onSprite : _offSprite;
    }
}

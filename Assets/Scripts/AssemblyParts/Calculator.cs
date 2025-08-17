using UnityEngine;

public class Calculator : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    private RandomSoundPlayer player;

    private void Start()
    {
        player = transform.GetComponent<RandomSoundPlayer>();
    }

    public void SetPowered(bool value)
    {
        GetComponent<SpriteRenderer>().sprite = value ? _onSprite : _offSprite;
        if (value) player.StartSounds();
        else player.StopSounds();
    }
}

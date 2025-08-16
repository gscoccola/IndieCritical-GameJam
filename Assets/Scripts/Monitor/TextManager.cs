using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI TMP;
    [SerializeField] private Sounds _uniqueSounds;

    private void Awake()
    {
        Instance = this;
    }

    public void SetText(string text)
    {
        TMP.text = text;
        _uniqueSounds.PlaySoundContainer("DisplayChange");

    }
}

using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI TMP;

    private void Awake()
    {
        Instance = this;
    }

    public void SetText(string text)
    {
        TMP.text = text;
    }
}

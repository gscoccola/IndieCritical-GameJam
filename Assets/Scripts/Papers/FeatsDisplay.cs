using UnityEngine;

public class FeatsDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _seals;

    private void Start()
    {
        CompletionInfo.Instance.sealsDisplay = this;
        DisplaySeals();
    }

    public void DisplaySeals()
    {
        for (int i = 0; i < _seals.Length; i++)
        {
            if (i >= CompletionInfo.Instance.FeatsCompletion.Length)
            {
                Debug.LogError("More seals than feats completion states!");
                break;
            }
            _seals[i].enabled = CompletionInfo.Instance.FeatsCompletion[i];
        }
    }
}

using System.Linq;
using UnityEngine;

public class CompletionInfo : MonoBehaviour
{
    public bool ReachedCheckpoint;
    public int FeatsCount = 2;
    public bool[] FeatsCompletion;
    public static CompletionInfo Instance;
    public bool IsReloadingScene;
    public FeatsDisplay sealsDisplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //ReachedCheckpoint = PlayerPrefs.GetInt("CheckpointReached", 0) == 1;
            DontDestroyOnLoad(gameObject);
            LoadFeatsCompletion();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint()
    {
        if (IsReloadingScene) return;
        ReachedCheckpoint = true;
        Debug.Log("Checkpoint reached");
        //PlayerPrefs.SetInt("CheckpointReached", 1);
    }

    public void SetFeatCompleted(int featIndex)
    {
        FeatsCompletion[featIndex] = true;
        sealsDisplay.DisplaySeals();
        PlayerPrefs.SetInt("Feat" + featIndex, 1);
        Debug.Log("Feat " + featIndex + " completed");
    }

    private void LoadFeatsCompletion()
    {
        //PlayerPrefs.DeleteAll();
        FeatsCompletion = new bool[FeatsCount];
        for (int i = 0; i < FeatsCount; i++)
        {
            if (!PlayerPrefs.HasKey("Feat" + i))
            {
                PlayerPrefs.SetInt("Feat" + i, 0);
                continue;
            }
            FeatsCompletion[i] = PlayerPrefs.GetInt("Feat" + i, 0) == 1;
        }
        Debug.Log("Feats completion loaded: " + string.Join(", ", FeatsCompletion.Select(b => b.ToString()).ToArray()));
        
    }
}

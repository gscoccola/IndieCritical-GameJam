using System.Linq;
using UnityEngine;

public class CheckpointInfo : MonoBehaviour
{
    public bool ReachedCheckpoint;
    public static CheckpointInfo Instance;
    public bool IsReloadingScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //ReachedCheckpoint = PlayerPrefs.GetInt("CheckpointReached", 0) == 1;
            DontDestroyOnLoad(gameObject);
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
}

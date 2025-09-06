using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool FinishedIntro;

    private void Start()
    {
        CompletionInfo.Instance.IsReloadingScene = false;

        if (!CompletionInfo.Instance.ReachedCheckpoint) return;

        var stateLoaders = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IStateLoader>();

        foreach (var stateLoader in stateLoaders)
        {
            stateLoader.LoadState();
        }
    }

    void OnRestart(InputValue value)
    {
        Restart();
    }

    void OnHardRestart(InputValue value)
    {
        HardRestart();
    }

    public void Restart()
    {
        CompletionInfo.Instance.IsReloadingScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HardRestart()
    {
        Debug.Log("Hard Restart");
        CompletionInfo.Instance.IsReloadingScene = true;

        CompletionInfo.Instance.ReachedCheckpoint = false;

        for (int i = 0; i < CompletionInfo.Instance.FeatsCompletion.Length; i++)
        {
            CompletionInfo.Instance.FeatsCompletion[i] = false;
        }
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

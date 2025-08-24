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

    public void Restart()
    {
        CompletionInfo.Instance.IsReloadingScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

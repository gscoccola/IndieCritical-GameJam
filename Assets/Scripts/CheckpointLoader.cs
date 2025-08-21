using System.Linq;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var stateLoaders = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IStateLoader>();

        foreach (var stateLoader in stateLoaders)
        {
            stateLoader.LoadState();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

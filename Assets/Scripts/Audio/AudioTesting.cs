using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioTesting : MonoBehaviour
{

    [SerializeField] private EventReference testReference;

    private EventInstance testInstance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        testInstance = RuntimeManager.CreateInstance(testReference);
        testInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

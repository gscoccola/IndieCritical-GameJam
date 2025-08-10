using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VelocityVolumeController : MonoBehaviour
{

    [Header("Volume Control")]
    [SerializeField] private Sounds _sounds;
    [SerializeField] private int _loopContainerID, _startContainerID, _stopContainerID;
    [SerializeField] private float _volumeMulti = 1, _minVolume = 0.1f, _maxVolume = 1f;
    private AudioSource _audioSource;
    private Vector2 _lastFramePosition;
    private bool _isPlaying, _hasMoved = false;

    [Header("Smoothing")]
    [Tooltip("The base speed the Display Slider changes")]
    [SerializeField] private float bufferAmtBase;
    [Tooltip("The speed at which the slider goes up. The higher the number the faster it reacts to changes")]
    [SerializeField] private float bufferSmoothUp;
    [Tooltip("The speed at which the slider goes down. The higher the number the faster it reacts to changes")]
    [SerializeField] private float bufferSmoothDown;
    private float buffer = 0f, bufferAmt, _diff = 0, curVolume = 0;
    private bool justLouder = false;
    public Slider debugSlider;

    public void StartVelocityControlledSound()
    {
        _isPlaying = true;
    }

    public void StopVelocityControlledSound()
    {
        _isPlaying = false;
        _hasMoved = false;
        _sounds.StopLoopingContainer(_loopContainerID);
        _sounds.PlaySoundContainer(_stopContainerID);
        _diff = 0;
    }

    private void Update()
    {

        if (_isPlaying)
        {
            _diff = _lastFramePosition.y - transform.position.y;

            if (_diff != 0 && !_hasMoved) 
            {
                _hasMoved = true;
                _sounds.PlayLoopingContainer(_loopContainerID, out _audioSource);
                _sounds.PlaySoundContainer(_startContainerID);
            }
            print(_diff);
            //_audioSource.volume = GetVolume(diff);
            if (_diff > 0.041f) _audioSource.volume = GetVolume(_diff); 
            //debugSlider.value = GetVolume(_diff);

            _lastFramePosition = transform.position;


        }

        _audioSource.volume -= Time.deltaTime;
        debugSlider.value -= Time.deltaTime;
    }



    private float GetVolume(float diff)
    {

        float targetVolume = MathF.Abs(diff * _volumeMulti);
        //print(targetVolume);


        if (!_isPlaying) 
        {
            return 0f;
        }

        if (targetVolume >= buffer)
        {
            if (!justLouder)
            {
                bufferAmt = bufferAmtBase;
                justLouder = true;
            }

            buffer += bufferAmt;
            bufferAmt *= bufferSmoothUp;

        }
        else if (targetVolume < buffer)
        {

            if (justLouder)
            {
                bufferAmt = bufferAmtBase;
                justLouder = false;
            }

            buffer -= bufferAmt;
            bufferAmt *= bufferSmoothDown;

        }
        debugSlider.value = buffer;
        return buffer;
    }

}

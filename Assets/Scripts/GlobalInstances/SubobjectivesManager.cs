using UnityEngine;

public class SubobjectivesManager : MonoBehaviour
{
    public static SubobjectivesManager  Instance;
    /*[SerializeField] SpriteRenderer _monitorSpriteRenderer;
    [SerializeField] Sprite _powerOnSprite;*/
    public bool PowerOn;
    public bool CPUOn;
    public bool GPUOn;
    private bool _wasEndingTriggered;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TextManager.Instance.SetText("NO POWER");
    }

    private void UpdateText()
    {
        if (_wasEndingTriggered) return;
        TextManager.Instance.SetText("POWER ON \n CPU " + BoolToString(CPUOn) + "\n GPU " + BoolToString(GPUOn));
    }

    public void SetPower()
    {
        //_monitorSpriteRenderer.sprite = _powerOnSprite;
        PowerOn = true;
        UpdateText();
    }

    public void SetCPU(bool value)
    {
        CPUOn = value;
        UpdateText();
        CheckEnding();
    }

    public void SetGPU(bool value)
    {
        GPUOn = value;
        UpdateText();
        CheckEnding();
    }

    public string BoolToString(bool value)
    {
        return value ? "ON" : "OFF";
    }

    private void CheckEnding()
    {
        if (CPUOn && GPUOn && PowerOn)
        {
            TriggerEnding(Endings.Fixed);
            
        }
    }

    public void TriggerEnding(Endings ending)
    {
        if (_wasEndingTriggered) return;
        _wasEndingTriggered = true;
        switch (ending)
        {
            case Endings.Fixed:
                TextManager.Instance.SetText("Ending 1/4: 'FIXED' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(0);
                break;
            case Endings.Ourobouros:
                TextManager.Instance.SetText("Ending 2/4: 'OUROBOUROS' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(1);
                break;

            case Endings.Freed:
                TextManager.Instance.SetText("Ending 3/4: 'FREED' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(2);
                break;
            case Endings.Softlocked:
                TextManager.Instance.SetText("Ending 4/4: 'SOFTLOCKED' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(3);
                break;
            default:
                break;
        }
    }
}

public enum Endings
{
    Fixed,
    Ourobouros,
    Freed,
    Softlocked,
}

using UnityEngine;

public class SubobjectivesManager : MonoBehaviour
{
    public static SubobjectivesManager  Instance;
    [SerializeField] SpriteRenderer _monitorSpriteRenderer;
    [SerializeField] Sprite _powerOnSprite;
    public bool PowerOn;
    public bool CPUOn;
    public bool GPUOn;

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
        TextManager.Instance.SetText("POWER ON \n CPU " + BoolToString(CPUOn) + "\n GPU " + BoolToString(GPUOn));
    }

    public void SetPower()
    {
        _monitorSpriteRenderer.sprite = _powerOnSprite;
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
        switch (ending)
        {
            case Endings.Fixed:
                TextManager.Instance.SetText("Ending 1/2: 'FIXED' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(0);
                break;
            case Endings.Ourobouros:
                TextManager.Instance.SetText("Ending 2/2: 'OUROBOUROS' \n Press R to restart");
                CompletionInfo.Instance.SetFeatCompleted(1);
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
}

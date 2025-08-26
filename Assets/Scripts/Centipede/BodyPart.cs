using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject _mouth;

    public void SetGrabbing(bool value)
    {
        _mouth.GetComponent<Mouth>().SetGrabbing(value);
    }
}

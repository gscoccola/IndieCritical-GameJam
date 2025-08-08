using System.Runtime.CompilerServices;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Assemblable _assemblable => GetComponent<Assemblable>();

    public bool IsPowered { get; private set; }

    private float initialScale;

    private void Start()
    {
        initialScale = transform.localScale.x;
    }

    private void Update()
    {
        if (!IsPowered && _assemblable.CurrentStatusEffects[StatusEffect.Electrified])
        {
            IsPowered = true;
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (IsPowered)
        {
            transform.localScale = (Mathf.PingPong(Time.time * 0.2f, 0.2f) * Vector3.one + Vector3.one) * initialScale;
        }
    }
}

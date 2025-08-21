using UnityEngine;

public interface IActivable
{
    void SetActive(bool value);
}

public interface IPowerable
{
    void SetPowered(StatusEffect statusEffect, bool value);
}

public interface IStateLoader
{
    void LoadState();
}
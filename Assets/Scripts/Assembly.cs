using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using System.Collections;

public class Assembly : Singleton<Assembly>
{
    public Assemblable[] Assembleables;
    public Cover[] Covers;
    public Array Connections;

    //private bool _propagateStatusEffects = true;

    [SerializeField] GameObject _sparkPrefab;

    private void Start()
    {
        StartCoroutine(IPropagateStatusEffects());
    }

    protected override void Awake()
    {
        base.Awake();
        Assembleables = FindObjectsByType<Assemblable>(FindObjectsSortMode.None);
        Covers = FindObjectsByType<Cover>(FindObjectsSortMode.None);
        Connections = new bool[Assembleables.Length, Assembleables.Length];
        for (int i = 0; i < Connections.GetLength(0); i++) Connections.SetValue(true, i, i);
    }

    void OnPrintConnections(InputValue value)
    {
        PrintBoolArray(Connections);
    }

    public void UpdateConnections(bool value, Assemblable i, Assemblable j)
    {
        Connections.SetValue(value, Array.IndexOf(Assembleables, i), Array.IndexOf(Assembleables, j));
        //if (_propagateStatusEffects) StartCoroutine(IPropagateStatusEffects());
    }

    private IEnumerator IPropagateStatusEffects()
    {
        while (true)
        {
            PropagateStatusEffect(StatusEffect.Electrified);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        /*_propagateStatusEffects = false;
        
        _propagateStatusEffects = true;*/
    }

    private void PrintBoolArray(Array array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                row += ((bool)array.GetValue(i, j)).ToInt();
            }
            Debug.Log(row);
        }
    }

    private void Spark(Vector3 position)
    {
        Instantiate(_sparkPrefab, position, Quaternion.identity);
    }

    private void PropagateStatusEffect(StatusEffect statusEffect)
    {
        List<int> queue = new List<int>();
        List<int> explored = new List<int>();

        foreach (Assemblable assemblable in Assembleables)
        {
            if (assemblable.StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Source)
            {
                queue.Add(Array.IndexOf(Assembleables, assemblable));
                assemblable.CurrentStatusEffects[statusEffect] = true;
            }
            else
            {
                assemblable.CurrentStatusEffects[statusEffect] = false;
            }
        }
        while (queue.Count > 0)
        {
            int currentIndex = queue[0];
            queue.RemoveAt(0);
            if (explored.Contains(currentIndex)) continue;
            explored.Add(currentIndex);
            Assemblable currentAssemblable = Assembleables[currentIndex];
            //currentAssemblable.CurrentStatusEffects[statusEffect] = true;
            int connections = currentAssemblable.StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Source ? 1 : 0;
            for (int i = 0; i < Connections.GetLength(1); i++)
            {
                if ((bool)Connections.GetValue(currentIndex, i) &&
                    Assembleables[i].StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Conductor)
                {
                    connections++;
                    if (explored.Contains(i)) continue;
                    Assembleables[i].CurrentStatusEffects[statusEffect] = true;
                    queue.Add(i);
                }
            }
            if (connections == 1) Spark(currentAssemblable.transform.position);
        }
    }
}


public static class BoolExtensions
{
    public static int ToInt(this bool value) => value ? 1 : 0;
}

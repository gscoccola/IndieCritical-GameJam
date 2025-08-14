using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using System.Collections;

public class Assembly : Singleton<Assembly>
{
    public Assemblable[] Assembleables;
    public Cover[] Covers;
    public bool[,] Connections;
    public VeinConnection[,] VeinConnections;

    //private bool _propagateStatusEffects = true;

    [SerializeField] GameObject _sparkPrefab;
    [SerializeField] GameObject _veinConnectionPrefab;

    private void Start()
    {
        StartCoroutine(IPropagateStatusEffects());
    }

    protected override void Awake()
    {
        base.Awake();
        Assembleables = FindObjectsByType<Assemblable>(FindObjectsSortMode.None);
        for (int i = 0; i < Assembleables.GetLength(0); i++) Assembleables[i].AssemblyIndex = i;
        Covers = FindObjectsByType<Cover>(FindObjectsSortMode.None);
        Connections = new bool[Assembleables.Length, Assembleables.Length];
        VeinConnections = new VeinConnection[Assembleables.Length, Assembleables.Length];
        for (int i = 0; i < Connections.GetLength(0); i++) Connections.SetValue(true, i, i);
    }

    void OnPrintConnections(InputValue value)
    {
        //PrintBoolArray(Connections);
        PrintRefArray(VeinConnections);
    }

    public void UpdateConnections(bool value, Assemblable assemblable1, Assemblable assemblable2)
    {
        int i = Array.IndexOf(Assembleables, assemblable1);
        int j = Array.IndexOf(Assembleables, assemblable2);
        Connections.SetValue(value, i, j);
        if (!value && VeinConnections[i, j] != null)
        {
            Destroy(VeinConnections[i, j].gameObject);
            VeinConnections[i, j] = null;
            
        }
    }

    private IEnumerator IPropagateStatusEffects()
    {
        while (true)
        {
            PropagateStatusEffect(StatusEffect.Electrified);
            yield return new WaitForSecondsRealtime(0.1f);
            PropagateStatusEffect(StatusEffect.BloodPowered);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private void PrintBoolArray(bool[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                row += array[i, j].ToInt();
            }
            Debug.Log(row);
        }
    }

    private void PrintRefArray(Array array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                row += array.GetValue(i, j) != null ? "1" : "0";
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
            int connections = currentAssemblable.StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Source ? 1 : 0;
            for (int i = 0; i < Connections.GetLength(1); i++)
            {
                if (Connections[currentIndex, i] &&
                    Assembleables[i].StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Conductor)
                {
                    if (statusEffect == StatusEffect.BloodPowered && VeinConnections[currentIndex, i] == null)
                    {
                        VeinConnections[currentIndex, i] = Instantiate(_veinConnectionPrefab, Vector3.zero, Quaternion.identity, transform)
                        .GetComponent<VeinConnection>();
                        VeinConnections[currentIndex, i].Setup(currentAssemblable, Assembleables[i]);
                    }

                    connections++;
                    if (explored.Contains(i)) continue;
                    Assembleables[i].CurrentStatusEffects[statusEffect] = true;
                    Assembleables[i].GetComponent<IPowerable>()?.SetPowered(statusEffect, true);
                    queue.Add(i);
                }
            }
            //if (connections == 1) Spark(currentAssemblable.transform.position);

        }
        for (int i = 0; i < Assembleables.Length; i++)
        {
            Assembleables[i].GetComponent<IPowerable>()?.SetPowered(statusEffect, Assembleables[i].CurrentStatusEffects[statusEffect]);
        }
    }
}


public static class BoolExtensions
{
    public static int ToInt(this bool value) => value ? 1 : 0;
}

/*
public class BoolArray
{
    private bool[,] _array;
    public BoolArray(int rows, int columns)
    {
        _array = new bool[rows, columns];
    }
    public bool this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }
    public int Length => _array.GetLength(0);
}*/
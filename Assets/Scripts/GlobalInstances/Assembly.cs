using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Assembly : MonoBehaviour
{
    public static Assembly Instance;
    public Assemblable[] Assembleables;
    public Cover[] Covers;
    public bool[,] Connections;
    public VeinConnection[,] VeinConnections;
    //public List<VeinConnection> VeinConnectionsFlat;

    //private bool _propagateStatusEffects = true;

    [SerializeField] GameObject _sparkPrefab;
    [SerializeField] GameObject _veinConnectionPrefab;
    private bool _makeNewVeins = true;
    private Dictionary<Tuple<int,StatusEffect>, bool> _poweredStatus = new();

    private void Start()
    {
        StartCoroutine(IPropagateStatusEffects());
    }

    protected void Awake()
    {
        Instance = this;
        Assembleables = FindObjectsByType<Assemblable>(FindObjectsSortMode.None);
        for (int i = 0; i < Assembleables.GetLength(0); i++) Assembleables[i].AssemblyIndex = i;
        Covers = FindObjectsByType<Cover>(FindObjectsSortMode.None);
        Connections = new bool[Assembleables.Length, Assembleables.Length];
        VeinConnections = new VeinConnection[Assembleables.Length, Assembleables.Length];
        for (int i = 0; i < Connections.GetLength(0); i++) Connections.SetValue(true, i, i);
    }

    /*void OnPrintConnections(InputValue value)
    {
        //PrintBoolArray(Connections);
        PrintRefArray(VeinConnections);
    }*/

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
            PropagateStatusEffect(StatusEffect.BloodPowered);
            yield return new WaitForEndOfFrame();
        }
    }

    /*private void PrintBoolArray(bool[,] array)
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
    }*/

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
                if (statusEffect== StatusEffect.Electrified) assemblable.SetElectrifiedVisual(false);

            }
        }
        while (queue.Count > 0)
        {
            int currentIndex = queue[0];
            queue.RemoveAt(0);
            if (explored.Contains(currentIndex)) continue;
            explored.Add(currentIndex);
            Assemblable currentAssemblable = Assembleables[currentIndex];
            //int connections = currentAssemblable.StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Source ? 1 : 0;
            for (int i = 0; i < Connections.GetLength(1); i++)
            {
                if (Connections[currentIndex, i] &&
                    Assembleables[i].StatusEffectAffinities[statusEffect] == StatusEffectAffinity.Conductor)
                {
                    if (statusEffect == StatusEffect.BloodPowered && VeinConnections[currentIndex, i] == null)
                    {
                        ConnectWithVein(currentIndex, i);   
                    }

                    //connections++;
                    if (explored.Contains(i)) continue;
                    Assembleables[i].CurrentStatusEffects[statusEffect] = true;
                    SetPowered(i, true, statusEffect);
                    Assembleables[i].SetElectrifiedVisual(true);
                    //if (UnityEngine.Random.Range(0, 5) == 1) Spark(currentAssemblable.transform.position);
                    /*if (Assembleables[i].GetComponent<SpriteRenderer>() != null)
                        Assembleables[i].GetComponent<SpriteRenderer>().color = Color.yellow;*/
                    if (statusEffect != StatusEffect.BloodPowered) queue.Add(i);
                }
            }
            //if (connections == 1) Spark(currentAssemblable.transform.position);

        }
        for (int i = 0; i < Assembleables.Length; i++)
        {
            SetPowered(i, Assembleables[i].CurrentStatusEffects[statusEffect], statusEffect);
        }
    }

    private void ConnectWithVein(int index1, int index2, bool randomize = false)
    {
        if (!_makeNewVeins) return;
        Debug.Log(Assembleables[index1] + " " + Assembleables[index2]);
        VeinConnections[index1, index2] = Instantiate(_veinConnectionPrefab, Vector3.zero, Quaternion.identity, transform)
                        .GetComponent<VeinConnection>();
        VeinConnections[index1, index2].Setup(Assembleables[index1], Assembleables[index2], randomize);
        //VeinConnectionsFlat.Add(VeinConnections[index1, index2]);
    }

    public void DeleteAllVeins()
    {
        _makeNewVeins = false;
        foreach (VeinConnection vein in VeinConnections)
        {
            if (vein != null)
            {
                //Debug.Log(vein);
                Destroy(vein.gameObject);
            }
                
        }
    }

    private void SetPowered(int index, bool value, StatusEffect statusEffect)
    {
        if (_poweredStatus.ContainsKey(new Tuple<int, StatusEffect>(index, statusEffect)) &&
            _poweredStatus[new Tuple<int, StatusEffect>(index, statusEffect)] == value) return;
        _poweredStatus[new Tuple<int, StatusEffect>(index, statusEffect)] = value;
        Assembleables[index].GetComponent<IPowerable>()?
            .SetPowered(statusEffect, Assembleables[index].CurrentStatusEffects[statusEffect]);
    }
}


public static class BoolExtensions
{
    public static int ToInt(this bool value) => value ? 1 : 0;
}

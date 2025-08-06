using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class Assembly : Singleton<Assembly>
{
    public Assemblable[] Assembleables;
    public Array Connections;

    private void Start()
    {
        Assembleables = FindObjectsByType<Assemblable>(FindObjectsSortMode.None);
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
}


public static class BoolExtensions
{
    public static int ToInt(this bool value) => value ? 1 : 0;
}


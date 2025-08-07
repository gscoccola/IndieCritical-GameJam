using UnityEngine;
using System.Collections.Generic;

public class ToolsManager : Singleton<ToolsManager>
{
    public Dictionary<ToolType, bool> Tools { get; private set; } = new();

    private void Start()
    {
        Tools[ToolType.Screwdriver] = false;
    }

    public void PickupTool(ToolType type)
    {
        Tools[type] = true;
    }
}

public enum ToolType
{
    Screwdriver,
}

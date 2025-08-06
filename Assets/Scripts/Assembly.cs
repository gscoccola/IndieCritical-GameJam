using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Assembly : Singleton<Assembly>
{
    public List<Assembleable> Assembleables = new List<Assembleable>();


}

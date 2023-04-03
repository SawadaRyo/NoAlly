using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolUseTrans:IObjectGenerator
{
    public Transform GenerateTrance { get; }
}

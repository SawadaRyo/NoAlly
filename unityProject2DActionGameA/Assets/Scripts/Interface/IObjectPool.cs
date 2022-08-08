using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool 
{
    public bool IsActive { get;}
    public void Create();
    public void Disactive();
    public void DisactiveForInstantiate();
}

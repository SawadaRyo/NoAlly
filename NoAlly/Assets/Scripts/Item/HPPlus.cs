using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPlus : ItemBase
{
    [SerializeField] int _HPPlusParameter = 4;
    public override void Activate(Collider other,PlayerStats gauge)
    {
        gauge.HPPuls(_HPPlusParameter);

    }
}

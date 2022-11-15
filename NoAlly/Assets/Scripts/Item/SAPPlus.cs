using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPPlus : ItemBase
{
    [SerializeField] int _SAPPlusParameter = 4;
    public override void Activate(Collider other,PlayerStatus gauge)
    {
        gauge.SAPPuls(_SAPPlusParameter);
    }
}

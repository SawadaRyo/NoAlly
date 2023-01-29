using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPPlus : ItemBase
{
    public override void Activate(HItParameter gauge)
    {
        base.Activate(gauge);
        gauge.BehaviorOfHit(this);
    }
}

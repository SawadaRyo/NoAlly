using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPPlus : PlusItemBase

{
    public override void Activate(HitParameter gauge)
    {
        base.Activate(gauge);
        gauge.BehaviorOfHit(this);
    }
}

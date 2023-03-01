using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    ObjectOwner Owner { get; }
    public void BehaviorOfHIt(float[] damageValue, ElementType type);
}

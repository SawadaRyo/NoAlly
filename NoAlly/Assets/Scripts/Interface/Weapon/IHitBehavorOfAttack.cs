using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    ObjectOwner Owner { get; }
    public void BehaviorOfHit(float[] damageValue, ElementType type);
}

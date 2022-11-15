using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    public float BehaviorOfHit(float weaponPower, float firePower, float elekePower, float frozenPower,ElementType type);
}

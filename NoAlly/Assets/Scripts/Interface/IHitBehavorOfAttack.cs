using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    public void BehaviorOfHit(float weaponPower, float firePower, float elekePower, float frozenPower,ElementType type);
}

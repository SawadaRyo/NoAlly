using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    ObjectOwner Owner { get; }
    public void BehaviorOfHit(WeaponBase weaponStatus, ElementType type);
}

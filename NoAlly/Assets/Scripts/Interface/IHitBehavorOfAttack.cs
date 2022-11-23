using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    public float BehaviorOfHit(WeaponBase weaponStatus, ElementType type);
}

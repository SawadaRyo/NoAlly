using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfAttack
{
    public void BehaviorOfHit(WeaponBase weaponStatus, ElementType type, WeaponOwner owner);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfGimic
{
    ObjectOwner Owner { get => ObjectOwner.GIMIC; }
    public void BehaviorOfHit(WeaponBase weaponBase,ElementType type);
}

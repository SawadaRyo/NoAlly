using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehavorOfGimic
{
    ObjectOwner Owner { get => ObjectOwner.GIMIC; }
    public void BehaviorOfHit<T>(T weaponBase,ElementType type) where T :WeaponBase;
}

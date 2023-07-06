using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeaponBase
{
    public ObjectOwner Owner { get; }
    public float[] WeaponPower { get; }
    public float[] ChargeLevels { get; }
    public ElementType ElementType { get; }
    public WeaponDeformation Deformated { get; }
    public void WeaponModeToElement(ElementType elementType);
    public void AttackMovement(Collider target,IWeaponAction weaponAction);
}

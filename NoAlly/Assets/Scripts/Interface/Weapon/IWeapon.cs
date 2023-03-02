using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeapon
{
    public float[] WeaponPower { get; }
    public WeaponDeformation Deformated { get; }
    public void WeaponAttackMovement(Collider target);
    public void WeaponMode(ElementType type);
}

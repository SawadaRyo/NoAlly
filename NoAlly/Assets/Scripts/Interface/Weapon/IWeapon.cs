using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeapon
{
    public float RigitPower { get; }
    public float ElekePower { get; }
    public float FirePower { get; }
    public float FrozenPower { get; }
    public WeaponDeformation Deformated { get; }
    public void WeaponAttackMovement(Collider target);
    public void WeaponMode(ElementType type);
}

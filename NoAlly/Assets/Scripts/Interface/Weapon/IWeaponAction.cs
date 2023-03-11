using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAction
{
    public float ChargeLevel1 { get; }
    public void WeaponAttack(Animator playerAnimator, WeaponProcessing weaponProcessing);
    public void WeaponChargeAttackMethod(float chrageCount, float[] weaponPower, ElementType elementType);
}

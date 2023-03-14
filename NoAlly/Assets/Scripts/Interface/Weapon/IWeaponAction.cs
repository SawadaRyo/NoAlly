using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAction
{
    public void WeaponAttack(Animator playerAnimator, WeaponProcessing weaponProcessing);
    public void WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, float[] weaponPower, ElementType elementType);
}

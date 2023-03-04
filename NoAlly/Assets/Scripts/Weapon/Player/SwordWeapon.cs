using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : CombatWeapon
{
    public override void WeaponMode(WeaponType weaponType,ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.FIRE:
                _isDeformated = WeaponDeformation.Deformation;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                _weaponAnimator.SetBool("IsOpen", false);
                break;
        }
        base.WeaponMode(weaponType,elementType);
    }
}

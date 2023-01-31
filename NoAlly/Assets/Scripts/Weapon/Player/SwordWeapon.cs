using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : CombatWeapon
{
    public override void Initialize(WeaponDataEntity weapon)
    {
        base.Initialize(weapon);
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.FIRE:
                _isDeformated = WeaponDeformation.Deformation;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.None;
                _weaponAnimator.SetBool("IsOpen", false);
                break;
        }
        base.WeaponMode(type);
    }
}

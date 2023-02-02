using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceWeapon : CombatWeapon, IWeapon
{
    float _normalRigit = 0f;
    float _normalFrozen = 0f;
    float _powerUpRigit = 3.5f;
    float _powerUpFrozen = 5f;

    public override void Initialize(WeaponDataEntity weapon)
    {
        base.Initialize(weapon);
        _normalRigit = _rigitPower;
        _normalFrozen = _frozenPower;
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.FROZEN:
                _isDeformated = WeaponDeformation.Deformation;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                _weaponAnimator.SetBool("IsOpen", false);
                break;
        }
        base.WeaponMode(type);
    }
}

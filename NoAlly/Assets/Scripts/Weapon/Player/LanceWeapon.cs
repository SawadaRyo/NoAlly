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
        _normalHarfExtents = new Vector3(0.1f, 0.8f, 1f);
        _pawerUpHarfExtents = new Vector3(0.45f, 1.1f, 1f);
        _harfExtents = _normalHarfExtents;
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.FROZEN:
                _isDeformated = WeaponDeformation.Deformation;
                _harfExtents = _pawerUpHarfExtents;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.None;
                _harfExtents = _normalHarfExtents;
                break;
        }
        base.WeaponMode(type);
    }
}

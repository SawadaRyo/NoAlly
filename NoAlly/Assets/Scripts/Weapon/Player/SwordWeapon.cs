using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : CombatWeapon
{
    public override void Initialize(WeaponDataEntity weapon)
    {
        base.Initialize(weapon);
        _normalHarfExtents = new Vector3(0.11f, 1f, 0.1f);
        _pawerUpHarfExtents = new Vector3(0.12f, 1.1f, 0.1f);
        _harfExtents = _normalHarfExtents;
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.FIRE:
                _isDeformated = WeaponDeformation.Deformation;
                _harfExtents = _pawerUpHarfExtents;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.None;
                _harfExtents = _normalHarfExtents;
                _weaponAnimator.SetBool("IsOpen", false);
                break;
        }
        base.WeaponMode(type);
    }
}

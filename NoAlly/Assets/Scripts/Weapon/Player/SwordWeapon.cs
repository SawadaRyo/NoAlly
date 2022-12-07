using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : CombatWeapon
{
    float _normalRigit = 0f;
    float _normalFire = 0f;
    float _powerUpRigit = 3.5f;
    float _powerUpFire = 5f;

    public override void Start()
    {
        base.Start();
        _normalRigit = _rigitPower;
        _normalFire = _elekePower;
        _normalHarfExtents = new Vector3(0.11f, 1f, 0.1f);
        _pawerUpHarfExtents = new Vector3(0.12f, 1.1f, 0.1f);
        _harfExtents = _normalHarfExtents;
    }
    public override void WeaponMode(ElementType type)
    {
        base.WeaponMode(type);

        switch (type)
        {
            case ElementType.FIRE:
                _isDeformated = true;
                _harfExtents = _pawerUpHarfExtents;
                _rigitPower = _powerUpRigit;
                _firePower = _powerUpFire;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = false;
                _harfExtents = _normalHarfExtents;
                _rigitPower = _normalRigit;
                _firePower = _normalFire;
                foreach (Renderer bR in _bladeRenderer)
                {
                    BladeFadeIn(bR);
                }
                break;
        }
    }
}

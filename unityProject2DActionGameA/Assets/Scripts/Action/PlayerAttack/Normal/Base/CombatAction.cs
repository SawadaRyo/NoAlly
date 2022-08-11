using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        var beforePower = _weaponBase.RigitPower;

        if (_chrageCount < _chargeLevel1) return;

        else if (_chrageCount >= _chargeLevel1 && _chrageCount < _chargeLevel2)
        {
            _weaponBase.RigitPower *= 1.5f;
        }
        else if (_chrageCount >= _chargeLevel2)
        {
            _weaponBase.RigitPower *= _chargeLevel2;
        }
        _animator.Play(_weaponName + "Chrage");
        _weaponBase.RigitPower = beforePower;
    }
}

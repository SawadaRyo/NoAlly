using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        if (_chrageCount < _chargeLevel1) return;

        else if (_chrageCount >= _chargeLevel1 && _chrageCount < _chargeLevel2)
        {
            _weaponBase.ChangePower(WeaponBase.TypeOfPower.RIGIT, 1.5f);
        }
        else if (_chrageCount >= _chargeLevel2)
        {
            _weaponBase.ChangePower(WeaponBase.TypeOfPower.RIGIT, _chargeLevel2);
        }
        _animator.Play(_weaponName + "Chrage");
        _weaponBase.ChangePower(WeaponBase.TypeOfPower.RIGIT, 1);
    }
}

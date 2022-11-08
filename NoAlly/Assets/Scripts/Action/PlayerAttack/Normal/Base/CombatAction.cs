using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        if (_chrageCount < _chargeLevel1)
        {
            _weaponBase.ChargePower(WeaponBase.TypeOfPower.RIGIT, 1);
        }
        else
        {
            if (_chrageCount >= _chargeLevel1 && _chrageCount < _chargeLevel2)
            {
                _weaponBase.ChargePower(WeaponBase.TypeOfPower.RIGIT, _chargeLevel1);
            }
            else if (_chrageCount >= _chargeLevel2)
            {
                _weaponBase.ChargePower(WeaponBase.TypeOfPower.RIGIT, _chargeLevel2);
            }
            _animator.Play(_weaponName + "Chrage");
        }
    }
}

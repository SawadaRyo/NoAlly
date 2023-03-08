using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod(float chrageCount, float[] weaponPower, ElementType elementType)
    {
        if (chrageCount < _chargeLevel1)
        {
            ChargePower(weaponPower, elementType, 1);
        }
        else if (chrageCount >= _chargeLevel1 && chrageCount < _chargeLevel2)
        {
            ChargePower(weaponPower, elementType, _chargeLevel1);
        }
        else if (chrageCount >= _chargeLevel2)
        {
            ChargePower(weaponPower, elementType, _chargeLevel2);
        }

    }
}

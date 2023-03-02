using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod()
    {
        if (_chrageCount < _chargeLevel1)
        {
            ChargePower(ElementType.RIGIT, 1);
        }
        else
        {
            if (_chrageCount >= _chargeLevel1 && _chrageCount < _chargeLevel2)
            {
                ChargePower(ElementType.RIGIT, _chargeLevel1);
            }
            else if (_chrageCount >= _chargeLevel2)
            {
                ChargePower(ElementType.RIGIT, _chargeLevel2);
            }
            //_animator.Play(_weaponName + "Chrage");
        }
    }
    public float ChargePower(ElementType top, float magnification)
    {
        float refPower = 0;
        switch (top)
        {
            case ElementType.RIGIT:
                refPower = _weaponBase.WeaponPower[(int)ElementType.RIGIT];
                break;
            case ElementType.FIRE:
                refPower = _weaponBase.WeaponPower[(int)ElementType.FIRE];
                break;
            case ElementType.ELEKE:
                refPower = _weaponBase.WeaponPower[(int)ElementType.ELEKE];
                break;
            case ElementType.FROZEN:
                refPower = _weaponBase.WeaponPower[(int)ElementType.FROZEN];
                break;
        }
        if (magnification < 1)
        {
            magnification = 1;
        }
        return refPower * magnification;
    }
}

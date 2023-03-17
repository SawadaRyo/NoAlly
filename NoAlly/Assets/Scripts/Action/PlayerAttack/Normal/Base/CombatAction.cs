using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction, ICombatAction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chrageCount">åªç›ÇÃó≠Çﬂéûä‘</param>
    /// <param name="chargeLevels">ó≠ÇﬂíiäK</param>
    /// <param name="weaponPower"></param>
    /// <param name="elementType"></param>
    public float[] CombatChargeAttackMethod(float chrageCount, float[] chargeLevels, float[] weaponPower, ElementType elementType)
    {
        if (chrageCount >= chargeLevels[0] && chrageCount < chargeLevels[1])
        {
            return ChargePower(weaponPower, elementType, chargeLevels[0]);
        }
        else if (chrageCount >= chargeLevels[1])
        {
            return ChargePower(weaponPower, elementType, chargeLevels[1]);
        }
        return ChargePower(weaponPower, elementType, 1);
    }
}

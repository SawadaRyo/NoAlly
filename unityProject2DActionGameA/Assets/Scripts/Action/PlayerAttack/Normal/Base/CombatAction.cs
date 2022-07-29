using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction
{
    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        var beforePower = m_weaponBase.RigitPower;

        if (m_chrageCount > m_chargeLevel1 && m_chrageCount <= m_chargeLevel2)
        {
            m_weaponBase.RigitPower *= m_chargeLevel1;
        }
        else if (m_chrageCount > m_chargeLevel2 && m_chrageCount <= m_chargeLevel3)
        {
            m_weaponBase.RigitPower *= m_chargeLevel2;
        }
        else if(m_chrageCount > m_chargeLevel3)
        {
            m_weaponBase.RigitPower *= m_chargeLevel3;
        }
        m_animator.Play(m_weaponName + "Chrage");
        m_weaponBase.RigitPower = beforePower;
    }
}

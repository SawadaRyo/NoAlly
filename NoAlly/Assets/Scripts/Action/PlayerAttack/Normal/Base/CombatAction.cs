using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : WeaponAction,ICombatAction
{
    LayerMask layer = 8;
    public virtual void HitMovement(Transform attackPos, Vector3 boxSize, IWeaponBase weaponBase)
    {
        RaycastHit[] targets = Physics.BoxCastAll(attackPos.position, boxSize, Vector3.zero, attackPos.rotation,layer);
        {
            foreach (RaycastHit target in targets)
            {
                if (target.collider.TryGetComponent(out IHitBehavorOfAttack characterHp))
                {
                    characterHp.BehaviorOfHIt(weaponBase.WeaponPower, _weaponBase.ElementType);
                }
                else if (target.collider.TryGetComponent(out IHitBehavorOfGimic hitObj))
                {
                    hitObj.BehaviorOfHit(_weaponBase, _weaponBase.ElementType);
                }
            }
        }
    }

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

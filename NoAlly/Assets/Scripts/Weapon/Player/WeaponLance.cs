using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLance : WeaponBase
{
    public WeaponLance(WeaponDataEntity weaponData,Transform attackpos) : base(weaponData,attackpos)
    {
    }

    public override void WeaponModeToElement(ElementType type)
    {
        switch (type)
        {
            case ElementType.FROZEN:
                _isDeformated = WeaponDeformation.Deformation;
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                break;
        }
        base.WeaponModeToElement(type);
    }

    public override void AttackMovement(Collider target,IWeaponAction weaponAction)
    {
        ICombatAction combatAction = weaponAction as ICombatAction;
        float[] weaponPower = combatAction.CombatChargeAttackMethod(weaponAction.ChargeCount, _chargeLevels, _weaponPower, _elementType);
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp))
        {
            characterHp.BehaviorOfHit(weaponPower, _elementType);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj))
        {
            hitObj.BehaviorOfHit(this, _elementType);
        }
        weaponAction.ChargeCount = 0f;
    }
}

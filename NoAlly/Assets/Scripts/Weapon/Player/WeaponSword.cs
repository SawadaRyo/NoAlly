using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : WeaponBase
{
    public WeaponSword(WeaponDataEntity weaponData,Transform attackPos) : base(weaponData,attackPos)
    {
        
    }

    public override void WeaponModeToElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.FIRE:
                _isDeformated = WeaponDeformation.Deformation;
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                break;
        }
        base.WeaponModeToElement(elementType);
    }
    public override void AttackMovement(Collider target, IWeaponAction weaponAction)
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

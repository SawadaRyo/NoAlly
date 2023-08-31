using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WeaponCombat : WeaponBase
{
    Vector3 _boxRenge = Vector3.zero;

    public override void Initializer(PlayerBehaviorController owner, WeaponController baseObj, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, baseObj, weaponData);
        _boxRenge = new Vector3(_base.GetAttackPos.localScale.x / 2
                              , _base.GetAttackPos.localScale.y / 2
                              , _base.GetAttackPos.localScale.z / 2);
    }
    public override void AttackBehaviour()
    {
        Collider[] cols = Physics.OverlapBox(_base.GetAttackPos.position
                                           , _boxRenge
                                           , _base.GetAttackPos.localRotation
                                           , _base.HitLayerToAttack);
        if (cols.Length == 0) return;
        _weaponPower = CurrentPower(InputCharging(_chargeCount));
        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out IHitBehavorOfAttack characterHp))
            {
                characterHp.BehaviorOfHit(_weaponPower, _base.CurrentElement.Value);
            }
            else if (col.TryGetComponent(out IHitBehavorOfGimic hitObj))
            {
                hitObj.BehaviorOfHit(this, _base.CurrentElement.Value);
            }
        }
    }

    public void DoParticle(BoolAttack flg)
    {
        if (flg == BoolAttack.ATTACKING)
        {
            _base.MyParticle.Play();
        }
        else if (flg == BoolAttack.NONE)
        {
            _base.MyParticle.Stop();
        }
    }
}

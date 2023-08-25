using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WeaponCombat : WeaponBase
{
    public override void Initializer(PlayerBehaviorController owner, WeaponController baseObj, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, baseObj, weaponData);
    }
    public override void AttackBehaviour()
    {
        //_owner.TargetCol
        //    .Where(target => _isEquipment && target != null)
        //    .Subscribe(target =>
        //    {
        //        _weaponPower = CurrentPower(InputCharging(_chargeCount));
        //        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp))
        //        {
        //            characterHp.BehaviorOfHit(_weaponPower, _owner.CurrentElement.Value);
        //        }
        //        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj))
        //        {
        //            hitObj.BehaviorOfHit(_owner, _owner.CurrentElement.Value);
        //        }
        //    });


        Collider[] cols = Physics.OverlapBox(_base.GetAttackPos.position
                                           , new Vector3(_base.GetAttackPos.localScale.x/2,_base.GetAttackPos.localScale.y/2, _base.GetAttackPos.localScale.z / 2)
                                           , _base.GetAttackPos.localRotation
                                           , _base.HitLayer);
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
                hitObj.BehaviorOfHit(_base, _base.CurrentElement.Value);
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

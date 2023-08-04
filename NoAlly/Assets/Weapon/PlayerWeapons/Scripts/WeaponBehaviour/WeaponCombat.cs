using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WeaponCombat : WeaponBase
{
    public override void Initializer(WeaponController owner, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, weaponData);
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


            Collider[] cols = Physics.OverlapBox(_owner.GetAttackPos.position
                                               , _owner.GetAttackPos.localScale
                                               , _owner.GetAttackPos.rotation
                                               , _owner.HitLayer);
            if (cols.Length == 0) return;
            _weaponPower = CurrentPower(InputCharging(_chargeCount));
            foreach (Collider col in cols)
            {
                if (col.TryGetComponent(out IHitBehavorOfAttack characterHp))
                {
                    characterHp.BehaviorOfHit(_weaponPower, _owner.CurrentElement.Value);
                }
                else if (col.TryGetComponent(out IHitBehavorOfGimic hitObj))
                {
                    hitObj.BehaviorOfHit(_owner, _owner.CurrentElement.Value);
                }
            }
    }

    public void DoParticle(BoolAttack flg)
    {
        if (flg == BoolAttack.ATTACKING)
        {
            _owner.MyParticle.Play();
        }
        else if (flg == BoolAttack.NONE)
        {
            _owner.MyParticle.Stop();
        }
    }
}

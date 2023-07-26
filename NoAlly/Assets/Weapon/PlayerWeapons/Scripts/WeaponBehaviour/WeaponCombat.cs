using UniRx;

public class WeaponCombat : WeaponBase
{
    public override void Initializer(WeaponController owner, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, weaponData);
        AttackBehaviour();
    }
    public override void AttackBehaviour()
    {
        _owner.TargetCol
            .Skip(1)
            .Subscribe(target =>
            {
                if (target.TryGetComponent(out IHitBehavorOfAttack characterHp))
                {
                    characterHp.BehaviorOfHit(_weaponPower, _owner.CurrentElement.Value);
                }
                else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj))
                {
                    hitObj.BehaviorOfHit(_owner, _owner.CurrentElement.Value);
                }
            });
    }

    public void AttackWeaponCombat(BoolAttack flg)
    {
        if (flg == BoolAttack.ATTACKING)
        {
            _owner.MyParticle.Play();
            _owner.WeaponPrefab.ActiveCollider(true);
        }
        else if (flg == BoolAttack.NONE)
        {
            _owner.MyParticle.Stop();
            _owner.WeaponPrefab.ActiveCollider(false);
        }
    }
}

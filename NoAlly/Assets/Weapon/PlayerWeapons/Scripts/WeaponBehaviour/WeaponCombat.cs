using UniRx;

public class WeaponCombat : WeaponBase
{

    public override void AttackBehaviour()
    {
        _owner.TargetCol
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

    public void WeaponCombatAttack(BoolAttack flg)
    {
        if (flg == BoolAttack.ATTACKING)
        {
            _owner.MyParticle.Play();
            //_weaponPrefab.ActiveCollider(true);
        }
        else if (flg == BoolAttack.NONE)
        {
            _owner.MyParticle.Stop();
            //_weaponPrefab.ActiveCollider(false);
        }
    }
}

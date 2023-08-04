using UnityEngine;

public class WeaponShield : WeaponBase
{
    public void Brocking()
    {

    }

    public override void AttackBehaviour()
    {
        Collider[] targets = Physics.OverlapBox(_owner.GetAttackPos.position
                                              , _owner.GetAttackPos.localScale
                                              , _owner.GetAttackPos.rotation
                                              , _owner.HitLayer);
        foreach (Collider targetCollider in targets)
        {
            //if(targetCollider.TryGetComponent(out IBullet<GunTypeEnemy> bullet))
            {
                //bullet.Disactive();
            }
        }
    }

    
}

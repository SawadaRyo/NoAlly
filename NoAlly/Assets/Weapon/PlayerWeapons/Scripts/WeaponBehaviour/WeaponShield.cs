using UnityEngine;

public class WeaponShield : WeaponBase
{
    Vector3 _hitRenge = Vector3.zero;

    public override void AttackBehaviour()
    {
        Collider[] targets = Physics.OverlapBox(_attackPos.position,_hitRenge, _attackPos.rotation);
        foreach(Collider targetCollider in targets)
        {
            //if(targetCollider.TryGetComponent(out IBullet<GunTypeEnemy> bullet))
            {
                //bullet.Disactive();
            }
        }
    }

    
}

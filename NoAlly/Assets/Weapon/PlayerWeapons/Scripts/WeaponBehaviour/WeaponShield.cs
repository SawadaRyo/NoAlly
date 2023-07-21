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

    public override void WeaponModeToElement(ElementType type)
    {
        switch (type)
        {
            case ElementType.ELEKE:
                _isDeformated = WeaponDeformation.Deformation;
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                break;
        }
        base.WeaponModeToElement(type);
    }
}

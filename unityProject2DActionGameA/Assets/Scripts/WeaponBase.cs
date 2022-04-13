using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class WeaponBase : MonoBehaviour
{
    [SerializeField] int m_weaponPower = 5;
    [SerializeField] int m_elekePower = 0;
    [SerializeField] int m_firePower = 0;
    [SerializeField] int m_frozenPower = 0;
    Collider m_weaponCollider = default;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }

    public void IsAttack(SphereCollider isAttack, LayerMask enemyLayer)
    {
        var enemiescol = Physics.OverlapSphere(isAttack.center, isAttack.radius, enemyLayer);
        foreach (var c in enemiescol)
        {
            var enemiesHP = gameObject.GetComponent<EnemyContoller>();

            if (enemiesHP)
            {
                enemiesHP.DamageMethod(WeaponPower,m_firePower,m_elekePower,m_frozenPower);
            }
        }
    }

    public void IsAttack(CapsuleCollider isAttack,LayerMask enemyLayer)
    {
        var direction = new Vector3 { [isAttack.direction] = 1 };
        var offset = isAttack.height / 2 - isAttack.radius;
        var localPoint0 = isAttack.center - direction * offset;
        var localPoint1 = isAttack.center + direction * offset;
        var enemiescol = Physics.OverlapCapsule(localPoint0, localPoint1, isAttack.radius, enemyLayer);
        foreach (var c in enemiescol)
        {
            var enemiesHP = gameObject.GetComponent<EnemyContoller>();

            if (enemiesHP)
            {
                enemiesHP.DamageMethod(WeaponPower, m_firePower, m_elekePower, m_frozenPower);
            }
        }
    }
}

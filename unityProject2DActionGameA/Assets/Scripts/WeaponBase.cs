using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class WeaponBase : MonoBehaviour
{
    [SerializeField] int m_weaponPower = 5;
    [SerializeField] int m_enchantPower = 2;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }

    public void IsAttack(Collider isAttack, float attackRenge, Vector3 attackCenter, LayerMask enemyLayer)
    {
        var enemiescol = Physics.OverlapSphere(attackCenter, attackRenge, enemyLayer);
        foreach (var c in enemiescol)
        {
            var enemiesHP = gameObject.GetComponent<EnemyContoller>();

            if (enemiesHP)
            {
                enemiesHP.DamageMethod(WeaponPower,m_enchantPower);
            }
        }
    }

    public void IsAttack(Collider isAttack, float attackRenge, Vector3 attackStart, Vector3 attackEnd, LayerMask enemyLayer)
    {
        var enemiescol = Physics.OverlapCapsule(attackStart, attackEnd, attackRenge, enemyLayer);
        foreach (var c in enemiescol)
        {
            var enemiesHP = gameObject.GetComponent<EnemyContoller>();

            if (enemiesHP)
            {
                enemiesHP.DamageMethod(WeaponPower,m_enchantPower);
            }
        }
    }
}

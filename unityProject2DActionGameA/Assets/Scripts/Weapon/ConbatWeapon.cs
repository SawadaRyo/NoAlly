using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConbatWeapon : WeaponBase
{
    public override void MovementOfWeapon(Collider target)
    {
        var targetHp = target.gameObject.GetComponent<PlayerGauge>();
        if(targetHp)
        {
            targetHp.DamageMethod(m_rigitPower, m_firePower, m_elekePower, m_frozenPower);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Collider targetHp))
        {
            MovementOfWeapon(targetHp);
        }
    }
}

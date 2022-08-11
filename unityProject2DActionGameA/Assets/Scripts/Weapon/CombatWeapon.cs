using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatWeapon : WeaponBase
{
    public override void MovementOfWeapon(Collider target)
    {
        var targetHp = target.gameObject.GetComponent<PlayerGauge>();
        if(targetHp)
        {
            targetHp.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
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

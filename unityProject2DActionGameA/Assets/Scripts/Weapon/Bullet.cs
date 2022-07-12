using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase
{
    float m_time = 0f;
    public override void Update()
    {
        m_time += Time.deltaTime;
        if(m_time > 5f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TargetObject" ||
           other.gameObject.GetComponent<EnemyGauge>() != null)
        {
            var damage = other.gameObject.GetComponent<EnemyGauge>();
            if(damage != null)
            {
                damage.DamageMethod(m_rigitPower, m_firePower, m_elekePower, m_frozenPower);
            }
        }
        Destroy(this.gameObject);
    }
}

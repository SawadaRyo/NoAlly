using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase
{
    float m_time = 0f;

    
    void Update()
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
           other.gameObject.GetComponent<GaugeManager>() != null)
        {
            var damage = other.gameObject.GetComponent<GaugeManager>();
            if(damage != null)
            {
                damage.DamageMethod(m_weaponPower, m_firePower, m_elekePower, m_frozenPower);
            }
        }
        Destroy(this.gameObject);
    }
}

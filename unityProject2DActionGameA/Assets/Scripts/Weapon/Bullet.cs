using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase
{

    [SerializeField] CapsuleCollider m_bulletCollider = default;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TargetObject" ||
           other.gameObject.tag == "BossEnemy")
        {
            IsAttack(m_bulletCollider);
            Destroy(this.gameObject);
        }
    }
}

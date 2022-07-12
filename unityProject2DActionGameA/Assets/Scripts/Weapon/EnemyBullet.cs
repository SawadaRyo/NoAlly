using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : WeaponBase
{
    [SerializeField] float m_bulletSpeed = 5;

    float m_time = 0f;
    Transform m_muzzleForward = default;
    Rigidbody m_rb = default;

    private void OnEnable()
    {
        m_rb = GetComponent<Rigidbody>();
        //m_muzzleForward = GameObject.FindGameObjectWithTag("EnemyMuzzle").transform;
        m_muzzleForward.position = transform.position;
    }
    public override void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > 5f)
        {
            Destroy(gameObject);
        }
    }
    public override void MovementOfWeapon()
    {
        base.MovementOfWeapon();
        m_rb.velocity = m_muzzleForward.transform.forward * m_bulletSpeed * m_time;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerGauge player))
        {
            player.DamageMethod(m_rigitPower, m_firePower, m_elekePower, m_frozenPower);
            Destroy(gameObject);
        }
    }

   
}

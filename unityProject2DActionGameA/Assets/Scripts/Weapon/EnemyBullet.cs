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
        m_rb.velocity = m_muzzleForward.transform.forward * m_bulletSpeed;
        if (m_time > 5f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHP player))
        {
            player.DamageMethod(m_weaponPower, m_firePower, m_elekePower, m_frozenPower);
            Destroy(gameObject);
        }
    }
}

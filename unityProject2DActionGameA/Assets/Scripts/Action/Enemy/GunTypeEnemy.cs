using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTypeEnemy : EnemyBase
{
    [SerializeField] int m_bulletSize = 50;
    [SerializeField] Transform m_attackPos;
    [SerializeField] EnemyBullet m_bulletPrefab;
    ObjectPool<EnemyBullet> m_bulletPool;
    float m_interval = 2f;


    public override void Start()
    {
        base.Start(); 
        m_bulletPool.SetBaseObj(m_bulletPrefab, m_attackPos);
        m_bulletPool.SetCapacity(m_bulletSize);
    }
    public override void EnemyAttack()
    {
        m_enemyAnimator.SetBool("Aiming", m_sightIn);
        StartCoroutine(RapidFire());
    }
    public void InsBullet()
    {
        m_bulletPool.Instantiate();
    }
    IEnumerator RapidFire()
    {
        var wait = new WaitForSeconds(m_interval);
        while (m_sightIn)
        {
            m_enemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }
}

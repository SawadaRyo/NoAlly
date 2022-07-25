using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTypeEnemy : EnemyBase
{
    [SerializeField] int m_bulletSize = 50;
    [SerializeField] Transform m_attackPos;
    [SerializeField] EnemyBullet m_bulletPrefab;
    ObjectPool<WeaponBase> m_bulletPool;
    float m_interval = 2f;


    public override void Start()
    {
        base.Start(); 
        m_bulletPool.SetBaseObj(m_bulletPrefab, m_attackPos);
        m_bulletPool.SetCapacity(m_bulletSize);
    }

  
    public override void EnemyAttack()
    {
        m_enemyAnimator.SetBool("Aiming", InSight());
        StartCoroutine(RapidFire(InSight()));
    }
    public void InsBullet()
    {
        m_bulletPool.Instantiate();
    }
    IEnumerator RapidFire(bool sightIn)
    {
        var wait = new WaitForSeconds(m_interval);
        while (sightIn)
        {
            m_enemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }
}

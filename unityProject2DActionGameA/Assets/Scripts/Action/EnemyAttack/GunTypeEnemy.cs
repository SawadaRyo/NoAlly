using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTypeEnemy : EnemyBase
{
    [SerializeField] int _bulletSize = 50;
    [SerializeField] Transform _attackPos;
    [SerializeField] Bullet _bulletPrefab;
    ObjectPool<Bullet> _bulletPool;
    float _interval = 2f;


    public override void Start()
    {
        base.Start(); 
        _bulletPool.SetBaseObj(_bulletPrefab, _attackPos);
        _bulletPool.SetCapacity(_bulletSize);
    }

  
    public override void EnemyAttack()
    {
        m_enemyAnimator.SetBool("Aiming", InSight());
        StartCoroutine(RapidFire(InSight()));
    }
    public void InsBullet()
    {
        _bulletPool.Instantiate();
    }
    IEnumerator RapidFire(bool sightIn)
    {
        var wait = new WaitForSeconds(_interval);
        while (sightIn)
        {
            m_enemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }
}

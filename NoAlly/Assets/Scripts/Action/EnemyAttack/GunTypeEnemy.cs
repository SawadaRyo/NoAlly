using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTypeEnemy : EnemyBase
{
    #region フィールド / プロパティ 群
    [SerializeField] int _bulletSize = 10;
    [SerializeField] Transform _muzzleTrans;
    [SerializeField] EnemyBullet _bulletPrefab;
    Vector3 _distance = Vector3.zero;
    const float _turnSpeed = 10f;
    [SerializeField] float _interval = 2f;
    ObjectPool<EnemyBullet> _bulletPool = new ObjectPool<EnemyBullet>();

    public Vector3 Distance => _distance;
    #endregion

    public override void Start()
    {
        base.Start();
        _bulletPool.SetBaseObj(_bulletPrefab, _muzzleTrans, (int)BulletOwner.Enemy);
        _bulletPool.SetCapacity(_bulletSize);
    }

    void EnemeyRotate()
    {
        Vector3 _distance = (PlayerContoller.Instance.transform.position - this.transform.position).normalized;
        if (_distance.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
        }
        else if (_distance.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
        }
    }

    public override void EnemyAttack()
    {
        if (InSight())
        {
            EnemeyRotate();
        }
        _enemyAnimator.SetBool("Aiming", InSight());
        StartCoroutine(RapidFire(InSight()));
    }
    public void InsBullet()
    {
        var bullet = _bulletPool.Instantiate();
        if(!bullet.EnemyMuzzleTrans)
        {
            bullet.EnemyMuzzleTrans = _muzzleTrans;
        }
    }
    IEnumerator RapidFire(bool sightIn)
    {
        var wait = new WaitForSeconds(_interval);
        while (sightIn)
        {
            _enemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }
}

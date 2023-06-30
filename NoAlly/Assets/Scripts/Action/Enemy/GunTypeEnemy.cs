using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
public class GunTypeEnemy : EnemyBase, IObjectGenerator
{
    [SerializeField,Header("生成する弾")] 
    EnemyBullet _bulletPrefab;
    [SerializeField,Header("生成する弾の数")]
    int _bulletSize = 10;
    [SerializeField,Header("弾の連射速度")] 
    float _interval = 1f;
    [SerializeField,Header("弾のマズル")] 
    Transform _muzzleTrans;
    [SerializeField,Header("弾の貯蓄地点")] 
    Transform _poolTrans;

    const float _turnSpeed = 0.5f;
    float _shootIntervalTime = 0;
    ObjectPool<EnemyBullet,GunTypeEnemy,EnemyBulletType> _bulletPool = new ObjectPool<EnemyBullet,GunTypeEnemy,EnemyBulletType>();
    ObjectPool<EnemyBullet, GunTypeEnemy, EnemyBulletType>.ObjectKey _key = null;
    Vector3 _distance = Vector3.zero;
    ActorVec _enemyVec = ActorVec.None; 

    public float Interval => _interval;
    public Transform GenerateTrance => _muzzleTrans;
    public Vector3 Distance => _distance;
    public ActorVec EnemyVec => _enemyVec; 

    public override void Start()
    {
        base.Start();
        _key = _bulletPool.SetBaseObj(_bulletPrefab, this, _poolTrans,EnemyBulletType.BULLET);
        _bulletPool.SetCapacity(_key, _bulletSize);
    }
    /// <summary>
    /// オブジェクトの回転制御
    /// </summary>
    /// <param name="player">向く方向</param>
    public override void EnemyRotate(Transform playerPos)
    {
        base.EnemyRotate(playerPos);
        if (_distance.x == 1)
        {
            this.transform.DORotate(new Vector3(0f, 90f, 0f), _turnSpeed);
            _enemyVec = ActorVec.Right;
        }
        else if (_distance.x == -1)
        {
            this.transform.DORotate(new Vector3(0f, -90f, 0f), _turnSpeed);
            _enemyVec = ActorVec.Left;
        }
    }

    public void InsBullet()
    {
        var bullet = _bulletPool.Instantiate(_key);
        //bullet.transform.position = _muzzleTrans.position;
    }

    public override void EnemyAttack()
    {
        _shootIntervalTime += Time.deltaTime;
        if(_shootIntervalTime > _interval)
        {
            EnemyAnimator.SetTrigger("Fire");
            _shootIntervalTime = 0;
        }
    }
}
enum EnemyBulletType
{
    BULLET
}


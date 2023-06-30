using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
public class GunTypeEnemy : EnemyBase, IObjectGenerator
{
    [SerializeField,Header("��������e")] 
    EnemyBullet _bulletPrefab;
    [SerializeField,Header("��������e�̐�")]
    int _bulletSize = 10;
    [SerializeField,Header("�e�̘A�ˑ��x")] 
    float _interval = 1f;
    [SerializeField,Header("�e�̃}�Y��")] 
    Transform _muzzleTrans;
    [SerializeField,Header("�e�̒��~�n�_")] 
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
    /// �I�u�W�F�N�g�̉�]����
    /// </summary>
    /// <param name="player">��������</param>
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


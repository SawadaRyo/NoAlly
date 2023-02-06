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
    ObjectPool<EnemyBullet> _bulletPool = new ObjectPool<EnemyBullet>();
    Vector3 _distance = Vector3.zero;

    public float Interval => _interval;
    public Transform GenerateTrance => _muzzleTrans;
    public Vector3 Distance => _distance;

    public override void Start()
    {
        base.Start();
        _bulletPool.SetBaseObj(_bulletPrefab, _poolTrans, (int)HitOwner.Enemy);
        _bulletPool.SetCapacity(this, _bulletSize);
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
        }
        else if (_distance.x == -1)
        {
            this.transform.DORotate(new Vector3(0f, -90f, 0f), _turnSpeed);
        }
    }

    public void InsBullet()
    {
        var bullet = _bulletPool.Instantiate((int)HitOwner.Enemy);
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
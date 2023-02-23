using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class BulletBase : WeaponBase, IObjectPool<IObjectGenerator>
{
    [SerializeField, Header("弾の速度")]
    float _bulletSpeed = 0;
    [SerializeField, Header("弾の残留時間")]
    float _intervalTime = 3f;
    [SerializeField, Header("弾のRigitbody")]
    Rigidbody _rb = default;
    [Tooltip("発射される直前の初期位置")]
    Transform _muzzlePos = null;
    [Tooltip("発射される方向")]
    Vector3 _muzzleForwardPos;
    [Tooltip("弾の加速度")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("弾の属性")]


    public IObjectGenerator Generator { get; private set; }

    float _time = 0f;

    public void FixedUpdate()
    {
        if (!_isActive) return;

        _time += Time.deltaTime;
        _velo.x = _bulletSpeed * _muzzleForwardPos.x;
        _velo.y = _muzzleForwardPos.y;
        _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0f);
        if (_time > _intervalTime)
        {
            Disactive();
        }
    }
    public override void WeaponAttackMovement(Collider target)
    {
        base.WeaponAttackMovement(target);
        if(target.TryGetComponent<IHitBehavorOfAttack>(out _))
        {
            Disactive();
        }
    }

    public void Create()
    {
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
        ActiveObject(true);
    }

    public void Disactive()
    {
        _rb.isKinematic = true;
        _time = 0f;
        ActiveObject(false);
    }
    public void DisactiveForInstantiate(IObjectGenerator Owner) 
    {
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
    }

    public void Disactive(float interval)
    {
        
    }
}



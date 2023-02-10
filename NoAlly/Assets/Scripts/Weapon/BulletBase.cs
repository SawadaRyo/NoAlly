using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class BulletBase : WeaponBase
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("’e‚ÌŽc—¯ŽžŠÔ")]
    float _intervalTime = 3f;
    [SerializeField, Header("’e‚ÌRigitbody")]
    Rigidbody _rb = default;
    [Tooltip("”­ŽË‚³‚ê‚é’¼‘O‚Ì‰ŠúˆÊ’u")]
    Transform _muzzlePos = null;
    [Tooltip("”­ŽË‚³‚ê‚é•ûŒü")]
    Vector3 _muzzleForwardPos;
    [Tooltip("’e‚Ì‰Á‘¬“x")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("’e‚Ì‘®«")]
    ElementType BulletElement { set => _type = value; }

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
        if(target.TryGetComponent<EnemyStatus>(out _))
        {
            Disactive();
        }
    }

    public override void Create()
    {
        base.Create();
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
    }

    public override void Disactive()
    {
        base.Disactive();
        _rb.isKinematic = true;
        _intervalTime = 0f;
    }
    public override void DisactiveForInstantiate<TOwner>(TOwner Owner)
    {
        base.DisactiveForInstantiate(Owner);

        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
    }
}



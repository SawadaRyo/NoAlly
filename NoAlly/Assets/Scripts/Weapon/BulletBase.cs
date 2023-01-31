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
    float __intervalTime = 0f;
    [SerializeField, Header("’e‚ÌRigitbody")]
    Rigidbody _rb = default;
    [Tooltip("”­ŽË‚³‚ê‚é’¼‘O‚Ì‰ŠúˆÊ’u")]
    Transform _muzzlePos = null;
    [Tooltip("”­ŽË‚³‚ê‚é•ûŒü")]
    Vector3 _muzzleForwardPos = Vector3.zero;
    [Tooltip("’e‚Ì‰Á‘¬“x")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("’e‚Ì‘®«")]
    ElementType _weaponEquipment;

    public void FixedUpdate()
    {
        if (!_isActive) return;

        __intervalTime += Time.deltaTime;
        _velo.x = _bulletSpeed * _muzzleForwardPos.x;
        _rb.velocity = new Vector3(_velo.x, _velo.y, 0f);
        if (__intervalTime > 3f)
        {
            Disactive();
        }
    }
    public override void WeaponAttackMovement(Collider target)
    {
        base.WeaponAttackMovement(target);
        {
            Disactive();
        }
    }

    public override void Create()
    {
        base.Create();
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.forward;
        this.transform.position = _muzzleForwardPos;
    }

    public override void Disactive()
    {
        base.Disactive();
        _rb.isKinematic = true;
        __intervalTime = 0f;
    }
    public override void DisactiveForInstantiate<TOwner>(TOwner Owner)
    {
        base.DisactiveForInstantiate(Owner);

        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
    }
}



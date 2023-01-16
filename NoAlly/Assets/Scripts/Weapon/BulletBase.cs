using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class BulletBase : WeaponBase, IObjectPool
{
    [SerializeField,Header("’e‚Ì‘¬“x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("’e‚ÌŽc—¯ŽžŠÔ")]
    float __intervalTime = 0f;
    [SerializeField, Header("’e‚ÌRigitbody")]
    Rigidbody _rb = default;
    [SerializeField, Header("’e‚ÌObjectVisual")]
    ObjectVisual _thisObject = null;

    [Tooltip("Žg—p’†‚©”»’è‚·‚é•Ï”")]
    bool _isActive = false;
    [Tooltip("”­ŽË‚³‚ê‚é’¼‘O‚Ì‰ŠúˆÊ’u")]
    Transform _muzzlePos = null;
    [Tooltip("”­ŽË‚³‚ê‚é•ûŒü")]
    Vector3 _muzzleForwardPos = Vector3.zero;
    [Tooltip("’e‚Ì‰Á‘¬“x")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("’e‚Ì‘®«")]
    ElementType _weaponEquipment;

    public bool IsActive => _isActive;

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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IHitBehavorOfAttack enemyHP))
        {
            enemyHP.BehaviorOfHit(this, _weaponEquipment, _owner);
        }
        else if (other.gameObject.TryGetComponent(out IHitBehavorOfGimic hitObj))
        {
            hitObj.BehaviorOfHit(_weaponEquipment);
        }
        Disactive();
    }

    public void Create()
    {
        _isActive = true;
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.forward;
        this.transform.position = _muzzleForwardPos;
        _thisObject.ActiveWeapon(_isActive);
    }

    public void Disactive()
    {
        _isActive = false;
        _rb.isKinematic = true;
        __intervalTime = 0f;
        _thisObject.ActiveWeapon(_isActive);
    }

    public void DisactiveForInstantiate<T>(T owner) where T : IObjectGenerator
    {
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _muzzlePos = owner.GenerateTrance;
        _velo = _rb.velocity;
        _thisObject.ActiveWeapon(_isActive);
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;

public class BulletBase : WeaponBase, IObjectPool
{
    [SerializeField] 
    float _bulletSpeed = 0;
    [SerializeField] 
    float __intervalTime = 0f;
    [SerializeField] 
    BulletOwner _owner = BulletOwner.Player;

    bool _isActive = false;
    Transform _muzzlePos = null;
    Rigidbody _rb = null;
    Vector3 _muzzleForwardPos = Vector3.zero;
    Vector3 _velo = Vector3.zero;
    Interval _interval = null;

    //テストコードなのでこのクラスとは関係ないよ
    public struct BB
    {
        float OO;
        private static readonly BB a = new BB(1f);
        public BB(float o)
        {
            OO = o;
        }
        public BB AA
        {
            //[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => a;
        }
    }

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
        if (_owner == BulletOwner.Player)
        {
            if (other.gameObject.TryGetComponent(out IHitBehavorOfAttack enemyHP))
            {
                enemyHP.BehaviorOfHit(this, MainMenu.Instance.Type);
            }
            else if (other.gameObject.TryGetComponent(out IHitBehavorOfGimic hitObj))
            {
                hitObj.BehaviorOfHit(MainMenu.Instance.Type);
            }
        }

        else if (_owner == BulletOwner.Enemy && other.gameObject.TryGetComponent(out IHitBehavorOfAttack playerHP))
        {
            //playerHP.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
        }
        Disactive();
    }

    public void Create()
    {
        _isActive = true;
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.forward;
        this.transform.position = _muzzleForwardPos;
        RendererActive(_isActive);
    }

    public void Disactive()
    {
        _isActive = false;
        _rb.isKinematic = true;
        __intervalTime = 0f;
        RendererActive(_isActive);
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _muzzlePos = GameObject.FindObjectOfType<BowAction>().MuzzlePos;
        _velo = _rb.velocity;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
    }
}
public enum BulletOwner
{
    Player = 0,
    Enemy = 1
}



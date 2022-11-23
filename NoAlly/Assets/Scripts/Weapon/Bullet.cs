using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase, IObjectPool
{
    [SerializeField] float _bulletSpeed = 0;
    [SerializeField] BulletOwner _owner = BulletOwner.Player;
    bool _isActive = false;
    float _time = 0f;
    Transform _muzzlePos = null;
    Rigidbody _rb = null;
    Vector3 _muzzleForwardPos = Vector3.zero;

    public bool IsActive => _isActive;

    public void FixedUpdate()
    {
        if (!_isActive) return;

        _time += Time.deltaTime;
        _rb.velocity = _bulletSpeed * _muzzleForwardPos;
        if (_time > 5f)
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

        if (_owner == BulletOwner.Enemy && other.gameObject.TryGetComponent(out IHitBehavorOfAttack playerHP))
        {
            playerHP.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
        }
        Disactive();
    }

    [System.Obsolete]
    public void Create()
    {
        _rb.isKinematic = false;
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        _isActive = true;
    }

    public void Disactive()
    {
        _rb.isKinematic = true;
        _time = 0f;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _isActive = false;
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _muzzlePos = GameObject.FindObjectOfType<BowAction>().MuzzlePos;
        _muzzlePos.TransformPoint(_muzzlePos.localPosition);
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



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
    Vector3 _muzzleForwardPos = Vector3.zero;
    Vector3 _velo = Vector3.zero;
    Rigidbody _rb = null;

    public bool IsActive => _isActive;

    public void FixedUpdate()
    {
        if (!_isActive) return;

        _time += Time.deltaTime;
        _velo.x = _bulletSpeed * _muzzleForwardPos.x;
        _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0f);
        if (_time > 5f)
        {
            Disactive();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (_owner == BulletOwner.Player && other.gameObject.TryGetComponent<EnemyStatus>(out EnemyStatus enemyHP))
        {
            enemyHP.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
        }
        else if (_owner == BulletOwner.Enemy && other.gameObject.TryGetComponent<PlayerGauge>(out PlayerGauge playerHP))
        {
            playerHP.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
        }
        else if(other.gameObject.tag == "TargetObject")
        {
            Disactive();
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
        _velo = _rb.velocity;
        _muzzlePos = GameObject.FindObjectOfType<BowAction>().MuzzlePos;
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



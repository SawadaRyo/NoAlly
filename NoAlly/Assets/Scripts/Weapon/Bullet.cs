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
    Rigidbody _rb = null;

    public bool IsActive => _isActive;

    public void FixedUpdate()
    {
        if (!_operated) return;

        _time += Time.deltaTime;
        this.transform.position += new Vector3(_bulletSpeed + _muzzleForwardPos.x, 0f, 0f);
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
        Disactive();
    }

    [System.Obsolete]
    public void Create()
    {
        _muzzleForwardPos = _muzzlePos.forward;
        this.transform.position = _muzzleForwardPos;
        _operated = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
    }

    public void Disactive()
    {
        _operated = false;
        _time = 0f;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
    }

    public void DisactiveForInstantiate()
    {
        _operated = false;
        _rb = GetComponent<Rigidbody>();
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



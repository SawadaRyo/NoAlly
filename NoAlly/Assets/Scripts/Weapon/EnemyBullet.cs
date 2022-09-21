using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : WeaponBase, IObjectPool
{
    [SerializeField] float _bulletSpeed = 5;

    float _time = 0f;
    Transform _enemyMuzzleTrans = default;
    Rigidbody _rb = default;

    public bool IsActive => _operation;
    public Transform EnemyMuzzleTrans { get => _enemyMuzzleTrans; set => _enemyMuzzleTrans = value; }

    public override void Update()
    {
        if (_operation)
        {
            _time += Time.deltaTime;
            if (_time > 5f)
            {
                Disactive();
            }
        }
    }
    public override void WeaponMovement()
    {
        base.WeaponMovement();
        _rb.velocity = _enemyMuzzleTrans.transform.forward * _bulletSpeed * _time;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerGauge player))
        {
            player.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
            Destroy(gameObject);
        }
    }

    public void Create()
    {
        this.transform.position = _enemyMuzzleTrans.position;
        _operation = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        _myCollider.enabled = true;
    }

    public void Disactive()
    {
        _operation = false;
        _time = 0f;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _myCollider.enabled = false;
    }

    public void DisactiveForInstantiate()
    {
        _rb = GetComponent<Rigidbody>();
        _operation = false;
        //_enemyMuzzleTrans = GameObject.FindObjectOfType<GunTypeEnemy>().MuzzleTrans;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _myCollider.enabled = false;
    }
}

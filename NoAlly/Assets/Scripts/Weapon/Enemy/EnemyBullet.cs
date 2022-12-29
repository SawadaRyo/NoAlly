using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : WeaponBase, IObjectPool
{
    [SerializeField] float _bulletSpeed = 5;

    float _time = 0f;
    Transform _enemyMuzzleTrans = default;
    Rigidbody _rb = default;
    Collider _myCollider = default;

    public bool IsActive => _operated;
    public Transform EnemyMuzzleTrans { get => _enemyMuzzleTrans; set => _enemyMuzzleTrans = value; }

    public override void Update()
    {
        if (_operated)
        {
            _time += Time.deltaTime;

            WeaponAttackMovement();
            if (_time > 5f)
            {
                Disactive();
            }
        }
    }
    public override void WeaponAttackMovement()
    {
        base.WeaponAttackMovement();
        _rb.velocity = _enemyMuzzleTrans.forward.normalized * _bulletSpeed;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IHitBehavorOfAttack player))
        {
            player.BehaviorOfHit(this, MainMenu.Instance.Type);
            Disactive();
        }
    }
    public void Create()
    {
        this.transform.position = _enemyMuzzleTrans.position;
        _operated = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        _myCollider.enabled = true;
    }
    public void Disactive()
    {
        _operated = false;
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
        _myCollider = GetComponent<BoxCollider>();
        _operated = false;
        _enemyMuzzleTrans = GetComponentInParent<GunTypeEnemy>().MuzzleTrans;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _myCollider.enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase, IObjectPool
{
    [SerializeField] float _bulletSpeed = 0;
    [SerializeField] BulletOwner _owner = default;
    [SerializeField] Collider _collider = default;
    bool _isActive = false;
    float _time = 0f;
    Transform _muzzulPos = default;

    public bool IsActive => _isActive;

    public void FixedUpdate()
    {
        if (!_operation) return;

        _time += Time.deltaTime;
        this.transform.position += new Vector3(_bulletSpeed + _muzzulPos.position.x, 0f, 0f) * _time;
        if (_time > 5f)
        {
            Disactive();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (_owner == BulletOwner.Player && other.gameObject.TryGetComponent<EnemyGauge>(out EnemyGauge enemyHP))
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
        this.transform.position = _muzzulPos.position;
        if(PlayerContoller.Instance.transform.rotation.y > 0)
        {
            this.transform.rotation = Quaternion.EulerRotation(0,0,90);
        }
        else if(PlayerContoller.Instance.transform.rotation.y < 0)
        {
            this.transform.rotation = Quaternion.EulerRotation(0,0,-90);
            _bulletSpeed *= -1;
        }
        _operation = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        _collider.enabled = true;
    }

    public void Disactive()
    {
        _operation = false;
        _time = 0f;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _collider.enabled = false;
    }

    public void DisactiveForInstantiate()
    {
        _operation = false;
        _muzzulPos = GameObject.FindObjectOfType<BowAction>().MuzzlePos;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _collider.enabled = false;
    }
}
public enum BulletOwner
{
    Player = 0,
    Enemy = 1
}



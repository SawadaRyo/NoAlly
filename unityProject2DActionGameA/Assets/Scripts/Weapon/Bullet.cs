using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase,IObjectPool
{
    [SerializeField] float _buletSpeed = 0;
    [SerializeField] BulletOwner _owner = default;
    bool _isActive = false;
    float _time = 0f;
    Collider _collider = default;

    public bool IsActive => _isActive;

    public override void Update()
    {
        if (!_isActive) return;

        _time += Time.deltaTime;
        this.transform.position += new Vector3(_buletSpeed * _time,0f,0f);
        if(_time > 5f)
        {
            Disactive();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(_owner == BulletOwner.Player && other.gameObject.TryGetComponent<EnemyGauge>(out EnemyGauge enemyHP))
        {
            enemyHP.DamageMethod(_rigitPower,_firePower,_elekePower,_frozenPower);
        }
        else if(_owner == BulletOwner.Enemy && other.gameObject.TryGetComponent<PlayerGauge>(out PlayerGauge playerHP))
        {
            playerHP.DamageMethod(_rigitPower,_firePower,_elekePower,_frozenPower);
        }
        Disactive();
    }
    public void Create()
    {
        _isActive = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        _collider.enabled = true;
    }

    public void Disactive()
    {
        _isActive = false;
        _time = 0f;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _collider.enabled = false;
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        _collider.enabled = false;
    }
}

public enum BulletOwner
{ 
    Player,
    Enemy,
}


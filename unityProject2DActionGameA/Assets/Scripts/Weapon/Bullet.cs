using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WeaponBase,IObjectPool
{
    bool _isActive = false;
    float m_time = 0f;
    float _buletSpeed = 0;
    Collider m_collider = default;
    BulletOwner _owner = default;

    public bool IsActive => _isActive;

    public override void Update()
    {
        m_time += Time.deltaTime;
        if(m_time > 5f)
        {
            Disactive();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyGauge>(out EnemyGauge enemyHP))
        {
            enemyHP.DamageMethod(_rigitPower,_firePower,_elekePower,_frozenPower);
        }
        else if(other.gameObject.TryGetComponent<PlayerGauge>(out PlayerGauge playerHP))
        {
            playerHP.DamageMethod(_rigitPower,_firePower,_elekePower,_frozenPower);
        }
        Destroy(this.gameObject);
    }
    public void Create()
    {
        _isActive = true;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        m_collider.enabled = true;
    }

    public void Disactive()
    {
        _isActive = false;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        m_collider.enabled = false;
    }

    public void DisactiveForInstantiate()
    {
        _isActive = false;
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        m_collider.enabled = false;
    }
}

public enum BulletOwner
{ 
    Player,
    Enemy,
}


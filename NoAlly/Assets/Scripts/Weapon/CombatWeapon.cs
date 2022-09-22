using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatWeapon : WeaponBase
{
    [SerializeField] Transform _center = default;
    [SerializeField] protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);

    bool _completedAttack = false;
    bool _attack = false;

    protected Vector3 _normalHarfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    protected Vector3 _pawerUpHarfExtents = new Vector3(0.4f, 1.7f, 0.1f);
    //public override void WeaponMovement(Collider target)
    //{
    //    var targetHp = target.gameObject.GetComponent<PlayerGauge>();
    //    if(targetHp)
    //    {
    //        targetHp.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.TryGetComponent(out Collider targetHp))
    //    {
    //        WeaponMovement(targetHp);
    //    }
    //}

    public override void Awake()
    {
        base.Awake();
        _myParticleSystem.Stop();
    }
    public void AttackOfCloseRenge(bool isAttack)
    {
        if (isAttack)
        {
            Collider[] enemiesInRenge = Physics.OverlapBox(_center.position, _harfExtents, Quaternion.identity, _enemyLayer);
            Debug.Log(enemiesInRenge.Length);
            if (enemiesInRenge.Length > 0)
            {
                foreach (Collider enemy in enemiesInRenge)
                {
                    if (enemy.TryGetComponent<EnemyGauge>(out EnemyGauge enemyGauge))
                    {
                        enemyGauge.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower);
                    }
                }
                _completedAttack = true;
            }
        }
    }
    public IEnumerator LoopJud(bool isAttack)
    {
        _attack = isAttack;
        _myParticleSystem.Play();
        while (_attack)
        {
            if (_completedAttack)
            {
                _completedAttack = false;
                _myParticleSystem.Stop();
                break;
            }
            else
            {
                AttackOfCloseRenge(true);
            }
            yield return null;
        }
        _completedAttack = false;
        _myParticleSystem.Stop();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(_center.position, _harfExtents);
    //}
}

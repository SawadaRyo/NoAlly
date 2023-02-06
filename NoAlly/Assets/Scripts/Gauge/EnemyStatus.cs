using UnityEngine;
using UniRx;

public class EnemyStatus: StatusBase //敵の体力を管理するスクリプト
{
    [Tooltip("敵のHP")]
    float _hp = 0;
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")]
    EnemyBase _enemyBase = null;

    public void Initialize(EnemyBase enemyBase)
    {
        base.Initialize();
        _enemyBase = enemyBase;
        _animator = _enemyBase.EnemyAnimator;
        _hp = _maxHP;
    }

    public override void Damage(WeaponBase weaponStatus, HitParameter difanse, ElementType type)
    {
        _hp -= base.DamageCalculation(weaponStatus, difanse, type);
        if (_hp <= 0)
        {
            Debug.Log("Death");
            _enemyBase.EnemyStateMachine.Dispatch((int)StateOfEnemy.Death);
        }
    }
}

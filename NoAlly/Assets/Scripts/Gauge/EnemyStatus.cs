using UnityEngine;
using UniRx;

public class EnemyStatus : StatusBase //敵の体力を管理するスクリプト
{
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")]
    EnemyBase _enemyBase = null;

    public override void Initialize()
    {
        base.Initialize();
        _enemyBase = GetComponent<EnemyBase>();
        _animator = _enemyBase.EnemyAnimator;
    }

    public override void DamageCalculation(WeaponBase weaponStatus, HItParameter difanse, ElementType type, HitOwner owner)
    {
        base.DamageCalculation(weaponStatus, difanse, type, owner);
        if (_hp.Value <= 0)
        {
            _enemyBase.EnemyStateMachine.Dispatch((int)EnemyState.Death);
        }
    }
}

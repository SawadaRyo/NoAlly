using UnityEngine;
using UniRx;

public class EnemyStatus : StatusBase //敵の体力を管理するスクリプト
{
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")]
    EnemyBase _enemyBase = null;

    public override void Initialize()
    {
        base.Initialize();
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    public override void DamageCalculation(WeaponBase weaponStatus, DifanseParameter difanse, ElementType type)
    {
        base.DamageCalculation(weaponStatus, difanse, type);
        if (_hp.Value <= 0)
        {
            _enemyBase.Disactive();
        }
        else return;
    }
}

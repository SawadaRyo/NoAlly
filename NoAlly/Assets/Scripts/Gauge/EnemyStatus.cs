using UnityEngine;
using UniRx;

public class EnemyStatus : StatusBase //敵の体力を管理するスクリプト
{
    [Tooltip("このオブジェクトにアタッチされているEnemyBaseを取得する変数")]
    EnemyBase _enemyBase = null;

    public override void Init()
    {
        base.Init();
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    //ダメージ計算
    public float DamageMethod(float weaponPower, float firePower, float elekePower, float frozenPower, ElementType type)
    {
        float baseDamage = weaponPower * _rigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = firePower* _fireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = elekePower* _elekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = frozenPower* _frozenDifansePercentage;
                break;
        }
        _hp.Value -= (baseDamage + elemantDamage);
        _audioSource.PlayOneShot(_damageSound);
        //Debug.Log(m_hp);
        //生死判定
        if (_hp.Value <= 0)
        {
            _enemyBase.Disactive();
        }
        return _hp.Value;
    }
}

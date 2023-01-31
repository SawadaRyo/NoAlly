using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StatusBase : ObjectVisual
{
    [SerializeField, Tooltip("無敵時間の値")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HPの上限")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("ダメージサウンド")]
    protected AudioClip _damageSound;
    [SerializeField, Header("このクラスのオーナー")]
    HitOwner _owner = HitOwner.NONE;



    [Tooltip("オブジェクトの生死判定")]
    protected bool _living = true;
    [Tooltip("Animatorの格納変数")]
    protected Animator _animator;
    [Tooltip("無敵時間")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSourceを格納する変数")]
    protected AudioSource _audioSource = null;
    bool _hitable = true;

    public bool Living => _living;

    public abstract void Damage(WeaponBase weaponStatus, HitParameter difanse, ElementType type);

    public virtual void Initialize()
    {
        _living = true;
        _invincibleTime = new Interval(_invincibleTimeValue);
        if (!GetComponentInChildren<HitParameter>())
        {
            Debug.LogError("当たり判定がありません");
        }
    }

    protected float DamageCalculation(WeaponBase weaponStatus, HitParameter difanse, ElementType type)
    {
        float baseDamage = weaponStatus.RigitPower * difanse.RigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = weaponStatus.FirePower * difanse.FireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = weaponStatus.ElekePower * difanse.ElekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = weaponStatus.FrozenPower * difanse.FrozenDifansePercentage;
                break;
            default:
                break;
        }
        StartCoroutine(StartCountDown());
        return(baseDamage + elemantDamage);
    }
    public IEnumerator StartCountDown()
    {
        while (true)
        {
            _hitable = false;
            if (_invincibleTime.IsCountUp())
            {
                _hitable = true;
                _invincibleTime.ResetTimer();
                break;
            }
            yield return null;
        }
    }
}

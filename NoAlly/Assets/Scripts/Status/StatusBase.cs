using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("無敵時間の値")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HPの上限")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("ダメージサウンド")]
    protected AudioClip _damageSound;


    [Tooltip("オブジェクトのHP")]
    protected float _hp = 0;
    [Tooltip("オブジェクトの生死判定")]
    protected bool _living = true;
    [Tooltip("Animatorの格納変数")]
    protected Animator _animator;
    [Tooltip("AudioSourceを格納する変数")]
    protected AudioSource _audioSource = null;
    [Tooltip("無敵時間")]
    protected Interval _invincibleTime = null;
    bool _hitable = true;

    public bool Living => _living;

    public virtual void Damage(WeaponPower damageValue, HitParameter difanse, ElementType type)
    {
        _hp -= DamageCalculation(damageValue, difanse, type);

    }

    public virtual void Initialize()
    {
        _living = true;
        _invincibleTime = new Interval(_invincibleTimeValue);
        _animator = GetComponent<Animator>();
        if (!GetComponentInChildren<HitParameter>())
        {
            Debug.LogError("当たり判定がありません");
        }
    }

    protected float DamageCalculation(WeaponPower damageValue, HitParameter difanse, ElementType type)
    {
        if (!_hitable) return 0;

        float baseDamage = damageValue.defaultPower * difanse.RigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = damageValue.elementPower * difanse.FireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = damageValue.elementPower * difanse.ElekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = damageValue.elementPower * difanse.FrozenDifansePercentage;
                break;
            default:
                break;
        }
        if (_hitable)
        {
            StartCoroutine(StartCountDown());
        }
        return (baseDamage + elemantDamage);
    }
    public IEnumerator StartCountDown()
    {
        _hitable = false;
        while (true)
        {
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

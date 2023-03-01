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

    public abstract void Damage(float[] damageValue, HitParameter difanse, ElementType type);

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

    protected float DamageCalculation(float[] damageValue, HitParameter difanse, ElementType type)
    {
        if (!_hitable) return 0;

        float baseDamage = damageValue[(int)ElementType.RIGIT] * difanse.RigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = damageValue[(int)ElementType.FIRE] * difanse.FireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = damageValue[(int)ElementType.ELEKE] * difanse.ElekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = damageValue[(int)ElementType.FROZEN] * difanse.FrozenDifansePercentage;
                break;
            default:
                break;
        }
        StartCoroutine(StartCountDown());
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

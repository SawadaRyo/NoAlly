using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("無敵時間の値")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HPの上限")]
    protected float _maxHP = 5; 
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("ダメージサウンド")]
    protected AudioClip _damageSound;
    [SerializeField,Header("このクラスのオーナー")]
    HitOwner _owner = HitOwner.NONE;



    [Tooltip("オブジェクトの生死判定")]
    protected bool _living = true;
    [Tooltip("Animatorの格納変数")]
    protected Animator _animator;
    [Tooltip("オブジェクトの現在のHP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("無敵時間")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSourceを格納する変数")]
    protected AudioSource _audioSource = null;

    public bool Living => _living;
    public IReadOnlyReactiveProperty<float> HP => _hp;
    bool Hitable => _invincibleTime.IsCountUp();

    
    public virtual void Initialize()
    {
        _living = true;
        _hp = new FloatReactiveProperty(_maxHP);
        _invincibleTime = new Interval(_invincibleTimeValue);
        if (!GetComponentInChildren<HItParameter>())
        {
            Debug.LogError("当たり判定がありません");
        }
    }

    public virtual void DamageCalculation(WeaponBase weaponStatus, HItParameter difanse, ElementType type,HitOwner owner)
    {
        if (_owner == owner || !Hitable) return;

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
        _hp.Value -= (baseDamage + elemantDamage);
        StartCoroutine(_invincibleTime.StartCountDown());
    }
}

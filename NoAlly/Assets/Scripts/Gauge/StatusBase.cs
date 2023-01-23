using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージサウンド")]
    protected AudioClip _damageSound;
    [SerializeField, Tooltip("無敵時間の値")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HPの上限")]
    protected float _maxHP = 5; 
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    protected float _maxSAP = 20;
    [SerializeField,Header("このクラスのオーナー")]
    WeaponOwner _owner = WeaponOwner.NONE;



    [Tooltip("オブジェクトの生死判定")]
    protected bool _living = true;
    [Tooltip("オブジェクトの現在のHP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("無敵時間")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSourceを格納する変数")]
    protected AudioSource _audioSource = null;
    [Tooltip("Animatorの格納変数")]
    protected Animator _animator;

    public bool Living => _living;
    public IReadOnlyReactiveProperty<float> HP => _hp;
    bool Hitable => _invincibleTime.IsCountUp();

    
    public virtual void Initialize()
    {
        _living = true;
        _hp = new FloatReactiveProperty(_maxHP);
        _invincibleTime = new Interval(_invincibleTimeValue);
        _animator = GetComponentInParent<Animator>();
        if (!GetComponentInChildren<DifanseParameter>())
        {
            Debug.LogError("HitAttckがありません");
        }
        StartCoroutine(_invincibleTime.StartCountDown());
    }

    public virtual void DamageCalculation(WeaponBase weaponStatus, DifanseParameter difanse, ElementType type,WeaponOwner owner)
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

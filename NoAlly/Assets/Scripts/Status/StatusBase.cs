using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("HP‚ÌãŒÀ")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("–³“GŽžŠÔ‚Ì’l")]
    protected float _invincibleTimeValue = 1f;
    [SerializeField]
    protected DamageEffects _damageEffects;


    [Tooltip("ƒIƒuƒWƒFƒNƒg‚ÌHP")]
    protected float _hp = 0;
    [Tooltip("ƒIƒuƒWƒFƒNƒg‚Ì¶Ž€”»’è")]
    protected bool _living = true;
    [Tooltip("Animator‚ÌŠi”[•Ï”")]
    protected Animator _animator;
    [Tooltip("AudioSource‚ðŠi”[‚·‚é•Ï”")]
    protected AudioSource _audioSource = null;
    [Tooltip("–³“GŽžŠÔ")]
    protected Interval _invincibleTime = null;
    bool _hitable = true;

    public bool Living => _living;

    public virtual void Damage(WeaponPower damageValue, ElementType type)
    {
        _hp -= DamageCalculation(damageValue, type);
    }
    public void ChangeParamater(float value,bool isPlus)
    {

    }
    public virtual void Death()
    {
        if (_hp <= 0)
        {
            Debug.Log("Death");
            _damageEffects.Death(_invincibleTimeValue);
        }
    }

    public virtual void Initialize()
    {
        _living = true;
        _invincibleTime = new Interval(_invincibleTimeValue);
        _animator = GetComponent<Animator>();
        if (!GetComponentInChildren<HitParameter>())
        {
            Debug.LogError("“–‚½‚è”»’è‚ª‚ ‚è‚Ü‚¹‚ñ");
        }
    }

    protected float DamageCalculation(WeaponPower damageValue, ElementType type)
    {
        if (_hitable)
        {
            StartCoroutine(StartCountDown());
            if (_damageEffects)
            {
                Debug.Log("DOC");
                _damageEffects.Damaged(_invincibleTimeValue,type);
            }
            return damageValue.defaultPower + damageValue.elementPower;
        }
        return 0f;
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

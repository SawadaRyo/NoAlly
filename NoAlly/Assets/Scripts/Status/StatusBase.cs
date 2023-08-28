using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("HP�̏��")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("���G���Ԃ̒l")]
    protected float _invincibleTimeValue = 1f;
    [SerializeField]
    protected DamageEffects _damageEffects;


    [Tooltip("�I�u�W�F�N�g��HP")]
    protected float _hp = 0;
    [Tooltip("�I�u�W�F�N�g�̐�������")]
    protected bool _living = true;
    [Tooltip("Animator�̊i�[�ϐ�")]
    protected Animator _animator;
    [Tooltip("AudioSource���i�[����ϐ�")]
    protected AudioSource _audioSource = null;
    [Tooltip("���G����")]
    protected Interval _invincibleTime = null;
    bool _hitable = true;

    public bool Living => _living;

    public virtual void Damage(WeaponPower damageValue, ElementType type)
    {
        _hp -= DamageCalculation(damageValue, type);
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
            Debug.LogError("�����蔻�肪����܂���");
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

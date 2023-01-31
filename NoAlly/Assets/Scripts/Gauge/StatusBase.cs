using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StatusBase : ObjectVisual
{
    [SerializeField, Tooltip("���G���Ԃ̒l")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HP�̏��")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("�_���[�W�T�E���h")]
    protected AudioClip _damageSound;
    [SerializeField, Header("���̃N���X�̃I�[�i�[")]
    HitOwner _owner = HitOwner.NONE;



    [Tooltip("�I�u�W�F�N�g�̐�������")]
    protected bool _living = true;
    [Tooltip("Animator�̊i�[�ϐ�")]
    protected Animator _animator;
    [Tooltip("���G����")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSource���i�[����ϐ�")]
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
            Debug.LogError("�����蔻�肪����܂���");
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

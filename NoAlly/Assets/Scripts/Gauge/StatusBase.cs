using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("���G���Ԃ̒l")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HP�̏��")]
    protected float _maxHP = 5; 
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
    protected float _maxSAP = 20;
    [SerializeField, Tooltip("�_���[�W�T�E���h")]
    protected AudioClip _damageSound;
    [SerializeField,Header("���̃N���X�̃I�[�i�[")]
    HitOwner _owner = HitOwner.NONE;



    [Tooltip("�I�u�W�F�N�g�̐�������")]
    protected bool _living = true;
    [Tooltip("Animator�̊i�[�ϐ�")]
    protected Animator _animator;
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("���G����")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSource���i�[����ϐ�")]
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
            Debug.LogError("�����蔻�肪����܂���");
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

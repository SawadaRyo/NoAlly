using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [SerializeField, Tooltip("�_���[�W�T�E���h")]
    protected AudioClip _damageSound;
    [SerializeField, Tooltip("���G���Ԃ̒l")]
    float _invincibleTimeValue = 1f;
    [SerializeField, Tooltip("HP�̏��")]
    protected float _maxHP = 5; 
    [SerializeField, Tooltip("�I�u�W�F�N�g�̕K�E�Z�Q�[�W�̏��")]
    protected float _maxSAP = 20;



    [Tooltip("�I�u�W�F�N�g�̐�������")]
    protected bool _living = true;
    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("�I�u�W�F�N�g�̌��݂̕K�E�Z�Q�[�W")]
    protected FloatReactiveProperty _sap = null;
    [Tooltip("���G����")]
    protected Interval _invincibleTime = null;
    [Tooltip("AudioSource���i�[����ϐ�")]
    protected AudioSource _audioSource = null;
    [Tooltip("Animator�̊i�[�ϐ�")]
    protected Animator _animator;

    public bool Living => _living;
    public bool IsInvincible => _invincibleTime.IsCountUp();
    public FloatReactiveProperty HP => _hp;

    private void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        _living = true;
        _hp = new FloatReactiveProperty(_maxHP);
        _sap = new FloatReactiveProperty(0);
        _invincibleTime = new Interval(_invincibleTimeValue);
        _animator = GetComponentInParent<Animator>();
        if (!GetComponentInChildren<DifanseParameter>())
        {
            Debug.LogError("HitAttck������܂���");
        }
        StartCoroutine(_invincibleTime.StartCountDown());
    }

    public virtual void DamageCalculation(WeaponBase weaponStatus, DifanseParameter difanse, ElementType type)
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
        _hp.Value -= (baseDamage + elemantDamage);
        StartCoroutine(_invincibleTime.StartCountDown());
    }
}

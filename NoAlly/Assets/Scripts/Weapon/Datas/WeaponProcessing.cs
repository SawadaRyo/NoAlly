using System;
using UnityEngine;
using UniRx;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : ObjectBase
{
    [SerializeField, Header("�v���C���[�̃A�j���[�^�[")]
    Animator _playerAnimator = null;
    [SerializeField, Header("����̎a���G�t�F�N�g")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("���C������ƃT�u����")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("�������Ă��镐��")]
    WeaponData _targetWeapon;
    float time = 0;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwichWeapon;

    private void Start()
    {
        _myParticleSystem.Stop();
    }
    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen && !PlayerAnimationState.Instance.IsAttack)
        {
            SwichWeapon(Input.GetButton("SubWeaponSwitch"));
        }
        WeaponAttack();
    }
    private void OnTriggerEnter(Collider other)
    {
        _targetWeapon.Action.HitMovement(other, _targetWeapon.Base);
    }
    /// <summary>
    /// ����̓��͔���
    /// </summary>
    void WeaponAttack()
    {
        if (!PlayerAnimationState.Instance.AbleInput || WeaponMenuHander.Instance.MenuIsOpen) return;
        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            _playerAnimator.SetTrigger("AttackTrigger");
            _playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
        }
        else
        {
            //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
            if (Input.GetButton("Attack"))
            {
                time += Time.deltaTime;
                _playerAnimator.SetBool("Charge", true);
                if (time > _targetWeapon.Action.ChargeLevel1)
                {
                    _playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            }
            else if (Input.GetButtonUp("Attack"))
            {
                _playerAnimator.SetBool("Charge", false);
                time = 0;
            }
        }


    }
    public void HitJud(BoolAttack isAttack)
    {
        switch (isAttack)
        {
            case BoolAttack.ATTACKING:
                _myParticleSystem.Play();
                ActiveCollider(true);
                break;
            default:
                _myParticleSystem.Stop();
                ActiveCollider(false);
                break;
        }
    }
    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
    /// </summary>
    public void SwichWeapon(bool weaponSwitch)
    {
        _targetWeapon.WeaponEnabled = false;
        if (!weaponSwitch)
        {
            _targetWeapon = _mainAndSub[(int)CommandType.MAIN];
        }
        else
        {
            _targetWeapon = _mainAndSub[(int)CommandType.SUB];
        }
        _targetWeapon.WeaponEnabled = true;
        _objectAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
    }
    /// <summary>
    /// ����̑���
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipment(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        _objectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
        if (_targetWeapon == null)
        {
            _targetWeapon = _mainAndSub[(int)type];
        }
    }
    public void SetElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.RIGIT:
                _objectAnimator.SetBool("IsOpen", false);
                break;
            default:
                _objectAnimator.SetBool("IsOpen", true);
                break;
        }
        _targetWeapon.Base.WeaponModeToElement(elementType);
    }
}






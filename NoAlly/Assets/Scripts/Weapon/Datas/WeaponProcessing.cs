using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Tooltip("����̃v���n�u")]
    ObjectBase _weaponPrefab = null;
    [SerializeField, Header("�v���C���[�̃A�j���[�^�[")]
    Animator _playerAnimator = null;
    [SerializeField, Header("����̎a���G�t�F�N�g")]
    ParticleSystem _myParticleSystem = default;

    ObservableStateMachineTrigger _trigger = null;
    [Tooltip("���C������ƃT�u����")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("�������Ă��镐��")]
    WeaponData _targetWeapon;

    public ObjectBase WeaponPrefab => _weaponPrefab;
    public ParticleSystem MyParticleSystem => _myParticleSystem;
    public WeaponData TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }

    private void Start()
    {
        _trigger = _weaponPrefab.ObjectAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        _myParticleSystem.Stop();
    }
    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen)
        {
            if (!PlayerAnimationState.Instance.IsAttack)
            {
                SwichWeapon(Input.GetButton("SubWeaponSwitch"));
            }
            WeaponAttack(_playerAnimator);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TargetWeapon.Base.AttackMovement(other, TargetWeapon.Action);
    }
    /// <summary>
    /// ����̓��͔���
    /// </summary>
    public void WeaponAttack(Animator playerAnimator)
    {
        if (!PlayerAnimationState.Instance.AbleInput || WeaponMenuHander.Instance.MenuIsOpen) return;
        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            playerAnimator.SetTrigger("AttackTrigger");
            playerAnimator.SetInteger("WeaponType", (int)TargetWeapon.Type);
        }
        else
        {
            //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
            if (Input.GetButton("Attack"))
            {
                TargetWeapon.Action.ChargeCount += Time.deltaTime;
                playerAnimator.SetBool("Charge", true);
                if (TargetWeapon.Action.ChargeCount > TargetWeapon.Base.ChargeLevels[0])
                {
                    playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            }
            else if (Input.GetButtonUp("Attack"))
            {
                playerAnimator.SetBool("Charge", false);
            }
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
        _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
    }
    /// <summary>
    /// ����̑���
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public void SetEquipment(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        if (_targetWeapon == null)
        {
            _targetWeapon = _mainAndSub[(int)type];
        }
        if (_targetWeapon.Type == _mainAndSub[(int)type].Type)
        {
            _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
        }
    }
    /// <summary>
    /// �����̑���
    /// </summary>
    /// <param name="elementType"></param>
    public void SetElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.RIGIT:
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", false);
                break;
            default:
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", true);
                break;
        }
        _targetWeapon.Base.WeaponModeToElement(elementType);
    }

}






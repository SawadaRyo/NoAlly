using System;
using UnityEngine;
using UniRx;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField,Tooltip("")]
    ObjectBase _weaponPrefab = null;
    [SerializeField, Header("�v���C���[�̃A�j���[�^�[")]
    Animator _playerAnimator = null;
    [SerializeField, Header("����̎a���G�t�F�N�g")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("���C������ƃT�u����")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("�������Ă��镐��")]
    WeaponData _targetWeapon;

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
        _targetWeapon.Action.WeaponAttack(_playerAnimator,this);
    }
    private void OnTriggerEnter(Collider other)
    {
        _targetWeapon.Base.HitMovement(other);
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
        _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
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
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", false);
                break;
            default:
                _weaponPrefab.ObjectAnimator.SetBool("IsOpen", true);
                break;
        }
        _targetWeapon.Base.WeaponModeToElement(elementType);
    }
}






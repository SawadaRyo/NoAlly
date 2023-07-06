using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �U�������؂�ւ��ȂǕ���ɂɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Tooltip("����̃v���n�u")]
    ObjectBase _weaponPrefab = null;
    [SerializeField, Header("�v���C���[�̃A�j���[�^�[")]
    Animator _playerAnimator = null;
    [SerializeField, Header("����̎a���G�t�F�N�g")]
    ParticleSystem _myParticleSystem = default;

    [Tooltip("���킪�ό`�����ǂ���")]
    bool _inDeformation = false;
    [Tooltip("����؂�ւ���")]
    BoolReactiveProperty _isSwtchWeapon = new BoolReactiveProperty();
    [Tooltip("����̃A�j���[�V�����̏��")]
    ObservableStateMachineTrigger _trigger = null;

    [Tooltip("���C������ƃT�u����")]
    WeaponData[] _mainAndSub = new WeaponData[2];
    [Tooltip("�������Ă��镐��")]
    WeaponData _targetWeapon;

    public ObjectBase WeaponPrefab => _weaponPrefab;
    public ParticleSystem MyParticleSystem => _myParticleSystem;
    public WeaponData TargetWeapon  => _targetWeapon;

    private void Start()
    {
        _myParticleSystem.Stop();
        WeaponAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        TargetWeapon.Base.AttackMovement(other, TargetWeapon.Action);
    }

    void WeaponAttack()
    {
        PlayerAnimationState.Instance.Hit
            .Subscribe(x =>
            {
                if(x == BoolAttack.ATTACKING)
                {
                    _myParticleSystem.Play();
                    _weaponPrefab.ActiveCollider(true);
                }
                else if(x == BoolAttack.NONE)
                {
                    _myParticleSystem.Stop();
                    _weaponPrefab.ActiveCollider(false);
                }
            });
    }

    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
    /// </summary>
    public WeaponData SwichWeapon(bool weaponSwitch)
    {
        if (_targetWeapon != null)
        {
            _targetWeapon.WeaponEnabled = false;
        }
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
        _playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
        return _targetWeapon;
    }
    /// <summary>
    /// ����̑���
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    public WeaponData SetEquipment(bool weaponSwitch)
    {
        if (!weaponSwitch && _mainAndSub[(int)CommandType.MAIN] != null)
        {
            _targetWeapon = _mainAndSub[(int)CommandType.MAIN];
            _targetWeapon.SetActAndBase(_mainAndSub[(int)CommandType.MAIN].Base, _mainAndSub[(int)CommandType.MAIN].Action);
        }
        else if (weaponSwitch && _mainAndSub[(int)CommandType.SUB] != null)
        {
            _targetWeapon = _mainAndSub[(int)CommandType.SUB];
            _targetWeapon.SetActAndBase(_mainAndSub[(int)CommandType.SUB].Base, _mainAndSub[(int)CommandType.SUB].Action);
        }
        Debug.Log(_targetWeapon);
        return _targetWeapon;
    }

    public void SetWeapon(WeaponData weaponType, CommandType type)
    {
        _mainAndSub[(int)type] = weaponType;
        //_targetWeapon = _mainAndSub[(int)type];
        //if (_targetWeapon.Type == _mainAndSub[(int)type].Type)
        //{
        //    _weaponPrefab.ObjectAnimator.SetInteger("WeaponType", (int)weaponType.Type);
        //    _playerAnimator.SetInteger("WeaponType", (int)_targetWeapon.Type);
        //}
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

    private void OnDisable()
    {
        _isSwtchWeapon.Dispose();
    }
}






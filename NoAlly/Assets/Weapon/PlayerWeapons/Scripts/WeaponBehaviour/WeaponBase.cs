using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase<PlayerBehaviorController>
{
    [Tooltip("����{��")]
    protected WeaponController _base = null;
    [Tooltip("���̕���̃f�[�^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("����̃I�[�i�[")]
    protected PlayerBehaviorController _owner = null;
    [Tooltip("��������")]
    protected bool _isEquipment = false;
    [Tooltip("���ߎ���")]
    protected float _chargeCount = 0f;
    [Tooltip("����̌��݂̍U����")]
    protected WeaponPower _weaponPower = WeaponPower.zero;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;

    public bool IsCharged => _chargeCount > _weaponData.ChargeLevels[0];
    public WeaponPower GetWeaponPower => _weaponPower;
    public WeaponDeformation Deformated => _isDeformated;
    public WeaponController Base => _base;
    public PlayerBehaviorController Owner => _owner;
    public WeaponDataEntity WeaponData => _weaponData;

    public virtual void Initializer(PlayerBehaviorController owner,WeaponController baseObj, WeaponDataEntity weaponData)
    {
        _owner = owner;
        _base = baseObj;
        _weaponData = weaponData;
    }
    /// <summary>
    /// ���͎��̋���
    /// </summary>
    public virtual void AttackBehaviour() { }
    public virtual void HitObjectBehaviour(Collider col) { }
    public void Charging(bool isInputCharge)
    {
        if (isInputCharge)
        {
            _chargeCount += Time.deltaTime;
        }
        else
        {
            if (IsCharged)
            {
                _weaponPower = CurrentPower(InputCharging(_chargeCount));
            }
            _chargeCount = 0;
        }
    }
    public virtual void WeaponModeToElement(ElementType type)
    {
        if (type == ElementType.RIGIT)
        {
            _isDeformated = WeaponDeformation.NONE;
        }
        else
        {
            _isDeformated = WeaponDeformation.Deformation;
        }
        _weaponPower.defaultPower = _weaponData.RigitPower[(int)_isDeformated];
        switch (_base.CurrentElement.Value)
        {
            case ElementType.FIRE:
                _weaponPower.elementPower = _weaponData.FirePower[(int)_isDeformated];
                break;
            case ElementType.ELEKE:
                _weaponPower.elementPower = _weaponData.ElekePower[(int)_isDeformated];
                break;
            case ElementType.FROZEN:
                _weaponPower.elementPower = _weaponData.FrozenPower[(int)_isDeformated];
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���߃R�}���h���̔���Ə���
    /// </summary>
    /// <param name="chargeTime">���ݗ��ߎ���</param>
    /// <returns>�U���̔{��</returns>
    public float InputCharging(float chargeTime)
    {
        float magnification = 1f;
        if (chargeTime > _weaponData.ChargeLevels[0])
        {
            magnification = _weaponData.ChargePowerLevels[0];
        }
        else if (chargeTime > _weaponData.ChargeLevels[1])
        {
            magnification = _weaponData.ChargePowerLevels[1];
        }
        return magnification;
    }
    /// <summary>
    /// ���݂̕���̍U����
    /// </summary>
    /// <param name="magnification">����̔{��</param>
    /// <returns>���݂̍U����</returns>
    public WeaponPower CurrentPower(float magnification = 1f)
    {
        WeaponPower weaponPower = WeaponPower.zero;
        weaponPower.defaultPower = _weaponData.RigitPower[(int)_isDeformated];
        switch (_base.CurrentElement.Value)
        {
            case ElementType.FIRE:
                weaponPower.elementPower = _weaponData.RigitPower[(int)_isDeformated];
                break;
            case ElementType.ELEKE:
                weaponPower.elementPower = _weaponData.RigitPower[(int)_isDeformated];
                break;
            case ElementType.FROZEN:
                weaponPower.elementPower = _weaponData.RigitPower[(int)_isDeformated];
                break;
            default:
                break;
        }
        if (magnification > 1f)
        {
            switch (_isDeformated)
            {
                case WeaponDeformation.NONE:
                    weaponPower.defaultPower *= magnification;
                    break;
                case WeaponDeformation.Deformation:
                    weaponPower.defaultPower *= 1.1f;
                    weaponPower.elementPower *= magnification;
                    break;
            }
        }
        return weaponPower;
    }
    /// <summary>
    /// ���������s����֐�
    /// </summary>
    public void OnEquipment()
    {
        _isEquipment = true;
        _weaponPower = CurrentPower();
    }
    /// <summary>
    /// �������������s����֐�
    /// </summary>
    public void OnLift()
    {
        _isEquipment = false;
        _chargeCount = 0f;
    }

}



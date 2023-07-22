using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase<WeaponController>
{
    [Tooltip("���ߎ���")]
    float _chargeCount = 0f;
    [Tooltip("����̃I�[�i�[")]
    protected WeaponController _owner = null;
    [Tooltip("")]
    protected WeaponPower _weaponPower = WeaponPower.zero;

    [Tooltip("�U������̒��S")]
    protected Transform _attackPos;
    [Tooltip("���̕���̃f�[�^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;

    public WeaponPower GetWeaponPower => _weaponPower;
    public WeaponDeformation Deformated => _isDeformated;
    public WeaponController WeaponOwner => _owner;
    public WeaponDataEntity WeaponData => _weaponData;

    public virtual void Initializer(WeaponController owner, WeaponDataEntity weaponData)
    {
        _owner = owner;
        _weaponData = weaponData;
    }
    public virtual void AttackBehaviour() { }
    public virtual void WeaponModeToElement(ElementType type)
    {
        if (type == _weaponData.ElementDeformation)
        {
            _isDeformated = WeaponDeformation.Deformation;
        }
        else
        {
            _isDeformated = WeaponDeformation.NONE;
        }
    }
    public bool Charge(bool isCharge)
    {
        if(isCharge)
        {
            _chargeCount += Time.deltaTime;
            if(_chargeCount > _weaponData.ChargeLevels[0])
            {
                return true;
            }
        }
        else
        {
            _chargeCount = 0f;
        }
        return false;
    }
    public WeaponPower CurrentPower(float magnification = 1f)
    {
        WeaponPower weaponPower = WeaponPower.zero;
        weaponPower.defaultPower = _weaponData.RigitPower[(int)_isDeformated];
        switch (_weaponData.ElementDeformation)
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
    public void OnEquipment()
    {
        
    }
    public void OnLift()
    {
        _chargeCount = 0f;
    }

}



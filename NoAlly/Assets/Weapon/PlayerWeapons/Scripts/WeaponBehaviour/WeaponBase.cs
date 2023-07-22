using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase<WeaponController>
{
    [Tooltip("溜め時間")]
    float _chargeCount = 0f;
    [Tooltip("武器のオーナー")]
    protected WeaponController _owner = null;
    [Tooltip("")]
    protected WeaponPower _weaponPower = WeaponPower.zero;

    [Tooltip("攻撃判定の中心")]
    protected Transform _attackPos;
    [Tooltip("この武器のデータ")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("武器が変形中かどうか")]
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



using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase<WeaponController>
{
    [Tooltip("装備判定")]
    protected bool _isEquipment = false;
    [Tooltip("溜め時間")]
    protected float _chargeCount = 0f;
    [Tooltip("武器のオーナー")]
    protected WeaponController _owner = null;
    [Tooltip("")]
    protected WeaponPower _weaponPower = WeaponPower.zero;

    [Tooltip("この武器のデータ")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("武器が変形中かどうか")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;

    public bool IsCharged => _chargeCount > _weaponData.ChargeLevels[0];
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
    }

    /// <summary>
    /// 溜めコマンド中の判定と処理
    /// </summary>
    /// <param name="chargeTime">現在溜め時間</param>
    /// <returns>攻撃の倍率</returns>
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
    /// 現在の武器の攻撃力
    /// </summary>
    /// <param name="magnification">武器の倍率</param>
    /// <returns>現在の攻撃力</returns>
    public WeaponPower CurrentPower(float magnification = 1f)
    {
        WeaponPower weaponPower = WeaponPower.zero;
        weaponPower.defaultPower = _weaponData.RigitPower[(int)_isDeformated];
        switch (_owner.CurrentElement.Value)
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
    /// 装備時実行する関数
    /// </summary>
    public void OnEquipment()
    {
        _isEquipment = true;
        _weaponPower = CurrentPower();
    }
    /// <summary>
    /// 装備解除時実行する関数
    /// </summary>
    public void OnLift()
    {
        _isEquipment = false;
        _chargeCount = 0f;
    }

}



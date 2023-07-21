using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData : MonoBehaviour
    {
        [Tooltip("")]
        Dictionary<WeaponType, WeaponBase> _weaponDataEntity = new();

        public Dictionary<WeaponType, WeaponBase> WeaponDatas => _weaponDataEntity;

        public void WeaponBaseInstantiate(WeaponController weaponOwner, WeaponScriptableObjects weaponData)
        {
            foreach (var d in weaponData.WeaponDatas)
            {
                WeaponBase weaponBase = null;
                try
                {
                    var t = Type.GetType(d.WeaponBaseName.ToString());
                    object obj = Activator.CreateInstance(t);
                    if (obj is WeaponCombat combat)
                    {
                        weaponBase = combat;
                    }
                    else if (obj is WeaponArrow arrow)
                    {
                        weaponBase = arrow;
                    }
                    else if (obj is WeaponShield shield)
                    {
                        weaponBase = shield;
                    }
                    else
                    {
                        Debug.LogError("想定されていないクラスです");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                
                if(weaponBase != null)
                {
                    weaponBase.Initializer(weaponOwner, d);
                    _weaponDataEntity.Add(d.TypeOfWeapon, weaponBase);
                }
            }
        }
    }
}

public class WeaponData
{
    WeaponBase _weaponBase;
    WeaponDataEntity _weaponDataEntity;

    public WeaponBase weaponBase => _weaponBase;
    public WeaponDataEntity weaponDataEntity => _weaponDataEntity;

    public WeaponData(WeaponDataEntity weaponDataEntity)
    {
        _weaponDataEntity = weaponDataEntity;

        try
        {
            var t = Type.GetType(_weaponDataEntity.WeaponBaseName.ToString());
            object obj = Activator.CreateInstance(t);
            if (obj is WeaponCombat combat)
            {
                _weaponBase = combat;
            }
            else if (obj is WeaponArrow arrow)
            {
                _weaponBase = arrow;
            }
            else if (obj is WeaponShield shield)
            {
                _weaponBase = shield;
            }
            else
            {
                Debug.LogError("想定されていないクラスです");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}



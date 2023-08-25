using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData
    {
        static SetWeaponData instance = null;
        [Tooltip("")]
        WeaponScriptableObjects _weaponScriptableObjects;
        [Tooltip("")]
        Dictionary<WeaponType, WeaponBase> _weaponDataEntity = new();

        public static SetWeaponData Instance
        {
            get => instance;
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }

        public SetWeaponData(WeaponScriptableObjects weaponScriptableObjects, PlayerBehaviorController owner, WeaponController baseObj)
        {
            _weaponScriptableObjects = weaponScriptableObjects;
            foreach (var d in _weaponScriptableObjects.WeaponDatas)
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

                if (weaponBase != null)
                {
                    weaponBase.Initializer(owner, baseObj, d);
                    _weaponDataEntity.Add(d.TypeOfWeapon, weaponBase);
                }
            }
        }

        public Dictionary<WeaponType, WeaponBase> WeaponDatas => _weaponDataEntity;


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



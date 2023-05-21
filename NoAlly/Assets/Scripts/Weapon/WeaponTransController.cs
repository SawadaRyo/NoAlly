using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponTransController : MonoBehaviour
{
    [Tooltip("管理する武器のプレハブ")]
    ObjectBase _targetWeapon = null;
    [Tooltip("利用する武器")]
    WeaponData _mainWeapon;
    [Tooltip("サブ配置")]
    WeaponData _subWeapon;


    public void FirstSetWeapon((WeaponData, WeaponData) weapons)
    {
        _mainWeapon = weapons.Item1;
        _subWeapon = weapons.Item2;
    }
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public (bool, bool) SwichWeapon(bool weaponSwitch)
    {
        _mainWeapon.WeaponEnabled = !weaponSwitch;
        _subWeapon.WeaponEnabled = weaponSwitch;

        return (_mainWeapon.WeaponEnabled, _subWeapon.WeaponEnabled);
    }

    public void SetEquipment(WeaponData weapon, CommandType type)
    {
        //_weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(false);
        //_weaponPrefabs[(int)_subWeapon.Type].ActiveObject(false);
        switch (type)
        {
            case CommandType.MAIN:
                _mainWeapon = weapon;
                break;
            case CommandType.SUB:
                _subWeapon = weapon;
                break;
        }
    }
}

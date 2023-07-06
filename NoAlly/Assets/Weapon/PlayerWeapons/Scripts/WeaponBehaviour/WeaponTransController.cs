using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponTransController : MonoBehaviour
{
    [Tooltip("�Ǘ����镐��̃v���n�u")]
    ObjectBase _targetWeapon = null;
    [Tooltip("���p���镐��")]
    WeaponData _mainWeapon;
    [Tooltip("�T�u�z�u")]
    WeaponData _subWeapon;


    public void FirstSetWeapon((WeaponData, WeaponData) weapons)
    {
        _mainWeapon = weapons.Item1;
        _subWeapon = weapons.Item2;
    }
    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
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

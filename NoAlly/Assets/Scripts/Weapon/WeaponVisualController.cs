using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField, Tooltip("武器の設置座標")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length - 1];

    [Tooltip("武器のプレハブ")]
    WeaponBase[] _weaponPrefabs = new WeaponBase[Enum.GetNames(typeof(WeaponType)).Length - 1];
    [Tooltip("メイン武器")]
    WeaponDatas _mainWeapon;
    [Tooltip("サブ配置")]
    WeaponDatas _subWeapon;

    public WeaponBase[] WeaponPrefabs => _weaponPrefabs;


    public void Initialize(SetWeaponData setWeapons, WeaponScriptableObjects weaponData,PlayerContoller player)
    {
        //武器のプレハブを生成&初期化
        for (int index = 0; index < Enum.GetNames(typeof(WeaponType)).Length - 1; index++)
        {
            _weaponPrefabs[index] = Instantiate(weaponData.WeaponDatas[index].Prefab, _weaponTransform[index]);
            WeaponAction action = _weaponPrefabs[index].GetComponent<WeaponAction>();
            action.Initialize(player, _weaponPrefabs[index]);
            _weaponPrefabs[index].SetData(weaponData.WeaponDatas[index]);
            setWeapons.SetAllWeapon[index] = new WeaponDatas(_weaponPrefabs[index], action, weaponData.WeaponDatas[index].Type);
        }
    }
    public void FirstSetWeapon((WeaponDatas, WeaponDatas) weapons)
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
        _weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(_mainWeapon.WeaponEnabled);
        _weaponPrefabs[(int)_subWeapon.Type].ActiveObject(_subWeapon.WeaponEnabled);

        return (_mainWeapon.WeaponEnabled, _subWeapon.WeaponEnabled);
    }

    public void SetEquipment(WeaponDatas weapon, CommandType type)
    {
        _weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(false);
        _weaponPrefabs[(int)_subWeapon.Type].ActiveObject(false);
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

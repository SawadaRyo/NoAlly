using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField,Tooltip("武器の設置座標")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length - 1];

    [Tooltip("武器のプレハブ")]
    WeaponBase[] _weaponPrefabs = new WeaponBase[Enum.GetNames(typeof(WeaponType)).Length - 1];
    [Tooltip("武器の挙動")]
    WeaponAction[] _weaponActions;
    [Tooltip("メイン武器")]
    WeaponDatas _mainWeapon;
    [Tooltip("サブ配置")]
    WeaponDatas _subWeapon;

    public WeaponBase[] WeaponPrefabs => _weaponPrefabs;


    public void Initialize(WeaponDatas[] weapons)
    {
        //武器のプレハブを生成
        for (int index = 0; index < weapons.Length; index++)
        {
            _weaponPrefabs[index] = Instantiate(weapons[index].Base, _weaponTransform[index]);
            _weaponPrefabs[index].Disactive();
        }
        //_mainWeapon = firstMainWeapon;
        //_subWeapon = firstSubWeapon;
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
        _weaponPrefabs[(int)_mainWeapon.Type] .Disactive();
        _weaponPrefabs[(int)_subWeapon.Type].Disactive();
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

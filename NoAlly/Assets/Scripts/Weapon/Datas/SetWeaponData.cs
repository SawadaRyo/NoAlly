using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData
    {
        [Tooltip("武器のスクリプタブルオブジェクト")]
        WeaponScriptableObjects weaponObjects = null;
        WeaponDatas[] _weaponDatas = null;

        public WeaponDatas[] GetAllWeapons => _weaponDatas;
        public WeaponDatas GetWeapon(WeaponType type) => _weaponDatas[(int)type];


        public SetWeaponData(WeaponScriptableObjects weapon,PlayerContoller player)
        {
            if (weaponObjects != null) return;

            weaponObjects = weapon;
            _weaponDatas = new WeaponDatas[weaponObjects.WeaponDatas.Length];
            for (int i = 0; i < weaponObjects.WeaponDatas.Length; i++)
            {
                _weaponDatas[i] = new WeaponDatas(weaponObjects.WeaponDatas[i]);
                _weaponDatas[i].Base.SetData(weaponObjects.WeaponDatas[i]);
                _weaponDatas[i].Action.Initialize(player, _weaponDatas[i].Base);
            }
        }
    }
}



public class WeaponDatas
{
    [Tooltip("武器を使用しているか")]
    bool _weaponEnabled;
    [Tooltip("武器本体")]
    WeaponBase _base = null;
    [Tooltip("武器のアクション")]
    WeaponAction _action = null;
    [Tooltip("武器の種類")]
    WeaponType _type = WeaponType.NONE;

    public bool WeaponEnabled { get => _weaponEnabled; set => _weaponEnabled = value; }
    public WeaponBase Base => _base;
    public WeaponAction Action => _action;
    public WeaponType Type => _type;

    public WeaponDatas(WeaponDataEntity weapon)
    {
        _base = weapon.Prefab;
        _action = weapon.Action;
        _type = weapon.Type;
    }
}


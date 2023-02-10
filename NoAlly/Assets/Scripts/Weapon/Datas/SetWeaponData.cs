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


        public void Initialize(WeaponScriptableObjects weapon, WeaponVisualController visualController,PlayerContoller player)
        {
            if (weaponObjects != null) return;

            weaponObjects = weapon;
            _weaponDatas = new WeaponDatas[weaponObjects.WeaponDatas.Entitys.Length];
            for (int i = 0; i < weaponObjects.WeaponDatas.Entitys.Length; i++)
            {
                _weaponDatas[i] = new WeaponDatas(visualController.WeaponPrefabs[i]
                                                , weaponObjects.WeaponDatas.Entitys[i].Action
                                                , weaponObjects.WeaponDatas.Entitys[i].Type);
                _weaponDatas[i].Base.SetData(weaponObjects.WeaponDatas.Entitys[i]);
                _weaponDatas[i].Action.Initialize(player, _weaponDatas[i].Base);
            }
        }
    }
}



public class WeaponDatas
{
    [Tooltip("武器を使用しているか")]
    bool _weaponEnabled;
    [Tooltip("武器の機能")]
    WeaponBase _base = null;
    [Tooltip("武器のアクション")]
    WeaponAction _action = null;
    [Tooltip("武器の種類")]
    WeaponType _type = WeaponType.NONE;

    public bool WeaponEnabled { get => _weaponEnabled; set => _weaponEnabled = value; }
    public WeaponBase Base => _base;
    public WeaponAction Action => _action;
    public WeaponType Type => _type;

    public WeaponDatas(WeaponBase @base, WeaponAction action,WeaponType type)
    {
        _base = @base;
        _action = action;
        _type = type;
    }
}


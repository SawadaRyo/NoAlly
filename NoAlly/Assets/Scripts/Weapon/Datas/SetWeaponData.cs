using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData
    {
        [Tooltip("武器のスクリプタブルオブジェクト")]
        WeaponScriptableObjects _weaponScriptableObjects = null;
        WeaponData[] _weaponDatas = null;

        public WeaponData[] SetAllWeapon { get => _weaponDatas; set => _weaponDatas = value; }
        public WeaponData[] GetAllWeapons => _weaponDatas;
        public WeaponData GetWeapon(WeaponType type) => _weaponDatas[(int)type];


        public SetWeaponData(WeaponScriptableObjects weapon)
        {
            if (_weaponScriptableObjects != null) return;
            _weaponScriptableObjects = weapon;
            IWeaponBase[] _weaponBases = new IWeaponBase[]
            {
                new WeaponSword(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.SWORD]),
                new WeaponLance(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.LANCE]),
                new WeaponSnip(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.BOW]),
                new WeaponShield(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.SHIELD])
            };
            IWeaponAction[] _weaponAction = new IWeaponAction[]
            {
                new CombatAction(),
                new CombatAction(),
                new CombatAction(),
                new ArrowAction(_weaponBases[(int)WeaponType.SHIELD])
            };

            for (int i = 0; i < _weaponDatas.Length; i++)
            {
                _weaponDatas[i] = new WeaponData(_weaponBases[i], _weaponAction[i], (WeaponType)i);
            }
        }
    }
}



public class WeaponData
{
    [Tooltip("武器を使用しているか")]
    bool _weaponEnabled;
    [Tooltip("武器本体")]
    IWeaponBase _base = null;
    [Tooltip("武器のアクション")]
    IWeaponAction _action = null;
    [Tooltip("武器の種類")]
    WeaponType _type = WeaponType.NONE;

    public bool WeaponEnabled { get => _weaponEnabled; set => _weaponEnabled = value; }
    public IWeaponBase Base => _base;
    public IWeaponAction Action => _action;
    public WeaponType Type => _type;

    public WeaponData(IWeaponBase @base, IWeaponAction action, WeaponType type)
    {
        _base = @base;
        _action = action;
        _type = type;
    }
}


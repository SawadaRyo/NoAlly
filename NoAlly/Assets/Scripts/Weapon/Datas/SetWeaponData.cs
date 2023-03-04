using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData
    {
        [Tooltip("����̃X�N���v�^�u���I�u�W�F�N�g")]
        WeaponScriptableObjects weaponObjects = null;
        WeaponDatas[] _weaponDatas = null;

        public WeaponDatas[] SetAllWeapon { get => _weaponDatas; set => _weaponDatas = value; }
        public WeaponDatas[] GetAllWeapons => _weaponDatas;
        public WeaponDatas GetWeapon(WeaponType type) => _weaponDatas[(int)type];


        public SetWeaponData(WeaponScriptableObjects weapon)
        {
            if (weaponObjects != null) return;

            weaponObjects = weapon;
            _weaponDatas = new WeaponDatas[weaponObjects.WeaponDatas.Length];
            for (int i = 0; i < weaponObjects.WeaponDatas.Length; i++)
            {
                _weaponDatas[i].Base.Initialize(weaponObjects.WeaponDatas[i]);
                _weaponDatas[i].Action.Initialize(_weaponDatas[i].Base);
            }
        }
    }
}



public class WeaponDatas
{
    [Tooltip("������g�p���Ă��邩")]
    bool _weaponEnabled;
    [Tooltip("����{��")]
    WeaponBase _base = null;
    [Tooltip("����̃A�N�V����")]
    WeaponAction _action = null;
    [Tooltip("����̎��")]
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
    public WeaponDatas(WeaponBase @base, WeaponAction action, WeaponType type)
    {
        _base = @base;
        _action = action;
        _type = type;
    }
}


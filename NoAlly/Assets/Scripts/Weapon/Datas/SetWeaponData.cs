using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("����̃X�N���v�^�u���I�u�W�F�N�g")]
    WeaponScriptableObjects _weaponObjects = null;
    WeaponDatas[] _weaponDatas = null;

    public WeaponScriptableObjects Datas => _weaponObjects;
    public WeaponDatas[] GetAllWeapons => _weaponDatas;
    public WeaponDatas GetWeapon(WeaponType type) => _weaponDatas[(int)type];


    public void Initialize(WeaponScriptableObjects weapon,PlayerContoller player)
    {
        if (_weaponObjects != null) return;

        _weaponObjects = weapon;
        _weaponDatas = new WeaponDatas[_weaponObjects.WeaponDatas.Entitys.Length];
        for (int i = 0; i < _weaponObjects.WeaponDatas.Entitys.Length; i++)
        {
            _weaponDatas[i] = new WeaponDatas(_weaponObjects.WeaponDatas.Entitys[i].Base
                                            , _weaponObjects.WeaponDatas.Entitys[i].Action
                                            , _weaponObjects.WeaponDatas.Entitys[i].Type);
            _weaponDatas[i].Base.Initialize(_weaponObjects.WeaponDatas.Entitys[i]);
            _weaponDatas[i].Action.Initialize(player, _weaponDatas[i].Base);
        }
    }
}

public class WeaponDatas
{
    [Tooltip("������g�p���Ă��邩")]
    bool _weaponEnabled;
    [Tooltip("����̋@�\")]
    WeaponBase _base = null;
    [Tooltip("����̃A�N�V����")]
    WeaponAction _action = null;
    [Tooltip("����̎��")]
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


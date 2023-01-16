using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponDatas = null;

    public WeaponScriptableObjects Datas => _weaponDatas;
    public WeaponDataEntity[] GetAllWeapons => _weaponDatas.WeaponDatas.Entitys;
    public WeaponDataEntity GetWeapon(WeaponType type) => _weaponDatas.WeaponDatas.Entitys[(int)type];


    public void Initialize(WeaponScriptableObjects weapon,PlayerContoller player)
    {
        if (_weaponDatas != null) return;

        _weaponDatas = weapon;
        for (int i = 0; i < _weaponDatas.WeaponDatas.Entitys.Length; i++)
        {
            _weaponDatas.WeaponDatas.Entitys[i].Base.Initialize(_weaponDatas.WeaponDatas.Entitys[i]);
            _weaponDatas.WeaponDatas.Entitys[i].Action.Initialize(player,_weaponDatas.WeaponDatas.Entitys[i].Base);
        }
    }
}

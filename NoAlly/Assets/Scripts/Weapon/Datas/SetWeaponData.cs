using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponDatas = null;

    public WeaponScriptableObjects Datas => _weaponDatas;
    public WeaponDateEntity[] GetAllWeapons => _weaponDatas.WeaponDatas.Entitys;
    public WeaponDateEntity GetWeapon(WeaponType type) => _weaponDatas.WeaponDatas.Entitys[(int)type];


    public void Initialize()
    {
        _weaponDatas = Resources.Load<WeaponScriptableObjects>("WeaponData");
        for (int i = 0; i < _weaponDatas.WeaponDatas.Entitys.Length; i++)
        {
            _weaponDatas.WeaponDatas.Entitys[i].Action.Initialize();
            _weaponDatas.WeaponDatas.Entitys[i].Base.Initialize();
        }
    }
}

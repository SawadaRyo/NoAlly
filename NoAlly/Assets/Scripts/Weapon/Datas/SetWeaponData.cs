using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("このクラスのインスタンス")]
    static SetWeaponData _instance;
    [Tooltip("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponDatas = null;

    public static SetWeaponData Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SetWeaponDatanのインスタンスがありません");
            }
            return _instance;
        }
    }
    public WeaponScriptableObjects WeaponDatas { get => _weaponDatas; set => _weaponDatas = value; }

    public Weapons GetAllWeapons()
    {
        return _weaponDatas.WeaponPrefab;
    }

    public WeaponDateEntity GetWeapon(WeaponType type)
    {
        return _weaponDatas.WeaponPrefab.Entitys[(int)type];
    }
}

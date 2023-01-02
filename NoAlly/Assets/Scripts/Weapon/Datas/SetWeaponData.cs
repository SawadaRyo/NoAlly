using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("このクラスのインスタンス")]
    static SetWeaponData _instance = new SetWeaponData();
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

    public void Initialize()
    {
        for(int i = 0;i < _weaponDatas.WeaponDatas.Entitys.Length;i++)
        {
            _weaponDatas.WeaponDatas.Entitys[i].Action.Initialize();
            _weaponDatas.WeaponDatas.Entitys[i].Base.Initialize();
        }
    }


    public WeaponDateEntity[] GetAllWeapons()
    {
        return _weaponDatas.WeaponDatas.Entitys;
    }

    public WeaponDateEntity GetWeapon(WeaponType type)
    {
        return _weaponDatas.WeaponDatas.Entitys[(int)type];
    }
}

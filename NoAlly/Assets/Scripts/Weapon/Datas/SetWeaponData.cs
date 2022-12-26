using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWeaponData
{
    [Tooltip("���̃N���X�̃C���X�^���X")]
    static SetWeaponData _instance;
    [Tooltip("����̃X�N���v�^�u���I�u�W�F�N�g")]
    WeaponScriptableObjects _weaponDatas = null;

    public static SetWeaponData Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SetWeaponDatan�̃C���X�^���X������܂���");
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataOfWeapon
{
    public class SetWeaponData : MonoBehaviour
    {
        [SerializeField]
        PlayerContoller _player;
        [SerializeField, Header("攻撃判定の中心点")]
        Transform _attackPos = null;
        [SerializeField, Header("攻撃判定の中心点")]
        Transform _poolPos = null;

        [Tooltip("武器のスクリプタブルオブジェクト")]
        WeaponScriptableObjects _weaponScriptableObjects = null;
        WeaponData[] _weaponDatas = new WeaponData[Enum.GetValues(typeof(WeaponType)).Length - 1];
        List<IWeaponBase> _weaponBases = null;
        List<IWeaponAction> _weaponAction = null;

        public WeaponData[] GetAllWeapons => _weaponDatas;
        public WeaponData GetWeapon(WeaponType type) => _weaponDatas[(int)type];


        public void Initialize(WeaponScriptableObjects weapon)
        {
            if (_weaponScriptableObjects != null) return;
            _weaponScriptableObjects = weapon;
            _weaponBases = new List<IWeaponBase>(){
                new WeaponSword(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.SWORD],_attackPos),
                new WeaponLance(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.LANCE],_attackPos),
                new WeaponArrow(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.ARROW],_player,_attackPos,_poolPos),
                new WeaponShield(_weaponScriptableObjects.WeaponDatas[(int)WeaponType.SHIELD], _attackPos)
            };
            _weaponAction = new List<IWeaponAction>(){
                new CombatAction(),
                new CombatAction(),
                new ArrowAction(),
                new CombatAction()
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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField,Tooltip("����̐ݒu���W")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length - 1];

    [Tooltip("����̃v���n�u")]
    WeaponBase[] _weaponPrefabs = new WeaponBase[Enum.GetNames(typeof(WeaponType)).Length - 1];
    [Tooltip("����̋���")]
    WeaponAction[] _weaponActions;
    [Tooltip("���C������")]
    WeaponDatas _mainWeapon;
    [Tooltip("�T�u�z�u")]
    WeaponDatas _subWeapon;

    public WeaponBase[] WeaponPrefabs => _weaponPrefabs;


    public void Initialize(WeaponDatas[] weapons)
    {
        //����̃v���n�u�𐶐�
        for (int index = 0; index < weapons.Length; index++)
        {
            _weaponPrefabs[index] = Instantiate(weapons[index].Base, _weaponTransform[index]);
            _weaponPrefabs[index].Disactive();
        }
        //_mainWeapon = firstMainWeapon;
        //_subWeapon = firstSubWeapon;
    }
    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
    /// </summary>
    public (bool, bool) SwichWeapon(bool weaponSwitch)
    {
        _mainWeapon.WeaponEnabled = !weaponSwitch;
        _subWeapon.WeaponEnabled = weaponSwitch;
        _weaponPrefabs[(int)_mainWeapon.Type].ActiveObject(_mainWeapon.WeaponEnabled);
        _weaponPrefabs[(int)_subWeapon.Type].ActiveObject(_subWeapon.WeaponEnabled);

        return (_mainWeapon.WeaponEnabled, _subWeapon.WeaponEnabled);
    }

    public void SetEquipment(WeaponDatas weapon, CommandType type)
    {
        _weaponPrefabs[(int)_mainWeapon.Type] .Disactive();
        _weaponPrefabs[(int)_subWeapon.Type].Disactive();
        switch (type)
        {
            case CommandType.MAIN:
                _mainWeapon = weapon;
                break;
            case CommandType.SUB:
                _subWeapon = weapon;
                break;
        }
    }
}

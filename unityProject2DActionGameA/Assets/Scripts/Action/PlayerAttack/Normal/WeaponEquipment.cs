using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


/// <summary>
/// 武器の装備を管理するコンポーネント
/// </summary>

public class WeaponEquipment : SingletonBehaviour<WeaponEquipment>
{
    [SerializeField] WeaponBase[] _weapons = default;

    [Tooltip("武器切り替え")] bool _weaponSwitch = false;
    [Tooltip("メイン武器")] WeaponBase _mainWeaponBase = default;
    [Tooltip("サブ武器")] WeaponBase _subWeaponBase = default;
    [Tooltip("装備中の武器")] WeaponBase _equipmentWeapon = default;
    [Tooltip("現在の属性")] ElementType _type = default;
    WeaponAction _weaponAction = default;
    ObjectPool<Bullet>[] _pool = new ObjectPool<Bullet>[4];

    public bool WeaponSwitch => _weaponSwitch;
    public WeaponAction EquipeWeaponAction => _weaponAction;

    private void Awake()
    {
        //TODO:武器設定をExcelから行えるようにする
        _mainWeaponBase = _weapons[0];
        _subWeaponBase = _weapons[1];

        ChangeWeapon(EquipmentType.MAIN, WeaponName.SWORD);
        ChangeWeapon(EquipmentType.SUB, WeaponName.LANCE);
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        _weaponAction = _equipmentWeapon.GetComponent<WeaponAction>();
    }


    void Update()
    {
        if (Input.GetButtonDown("WeaponChange")
            && !Input.GetButton("Attack"))
        {
            SwichWeapon();
        }

        if (!MenuHander.Instance.MenuIsOpen)
        {
            _weaponAction.WeaponAttack(_equipmentWeapon.name);
        }

        if (PlayerContoller.Instance.IsWalled())
        {
            _equipmentWeapon.RendererActive(false);
        }
        else
        {
            _equipmentWeapon.RendererActive(true);
        }
    }
    void SwichWeapon()
    {
        //武器のメインとサブの表示を切り替える

        _weaponSwitch = !_weaponSwitch;
        //_mainWeaponBase.RendererActive(_weaponSwitch);
        //_subWeaponBase.RendererActive(!_weaponSwitch);

        //メインとサブの武器を切り替える
        if (_weaponSwitch)
        {
            //_equipmentWeapon = _mainWeaponBase;
            SetEquipment(_mainWeaponBase, _subWeaponBase);
        }
        else
        {
            //_equipmentWeapon = _subWeaponBase;
            SetEquipment(_subWeaponBase, _mainWeaponBase);
        }
        _weaponAction = _equipmentWeapon.GetComponent<WeaponAction>();
    }
    public void ChangeWeapon(EquipmentType equipmentType, WeaponName? weaponName)
    {
        if (equipmentType == EquipmentType.MAIN)
        {
            if (_mainWeaponBase.IsActive)
            {
                SetEquipment(_weapons[(int)weaponName], _mainWeaponBase);
            }
            else
            {
                _mainWeaponBase = _weapons[(int)weaponName];
            }
        }

        else if (equipmentType == EquipmentType.SUB)
        {
            if (_subWeaponBase.IsActive)
            {
                SetEquipment(_weapons[(int)weaponName], _subWeaponBase);
            }
            else
            {
                _subWeaponBase = _weapons[(int)weaponName];
            }
        }
    }

    /// <summary>武器の装備を決める関数
    /// ・第一引数：装備させる武器
    /// ・第二引数：装備させていた武器</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="athorWeapon"></param>
    void SetEquipment(WeaponBase equipmentWeapon, WeaponBase athorWeapon)
    {
        _equipmentWeapon = equipmentWeapon;
        _equipmentWeapon.IsActive = true;
        athorWeapon.IsActive = false;
        _equipmentWeapon.RendererActive(true);
        athorWeapon.RendererActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipment : SingletonBehaviour<WeaponEquipment>
{
    [SerializeField] string _filePath = "";
    [SerializeField] WeaponBase[] _weapons = default;

    [Tooltip("武器切り替え")] bool _weaponSwitch = false;
    [Tooltip("メイン武器")] WeaponBase _mainWeaponBase = default;
    [Tooltip("サブ武器")] WeaponBase _subWeaponBase = default;
    [Tooltip("装備中の武器")] WeaponBase _equipmentWeapon = default;
    [Tooltip("現在の属性")] ElementType _type = default;
    WeaponAction _weaponAction = default;
    ObjectPool<Bullet>[] _pool = new ObjectPool<Bullet>[4];

    public WeaponBase EquipmentWeapon { get => _equipmentWeapon; set => _equipmentWeapon = value; }
    public WeaponBase MainWeapon { get => _mainWeaponBase; set => _mainWeaponBase = value; }
    public WeaponBase SubWeapon { get => _subWeaponBase; set => _subWeaponBase = value; }
    public WeaponAction EquipeWeaponAction => _weaponAction;

    private void Awake()
    {
        for (int x = 0; x < _weapons.Length; x++)
        {
            
        }
        _mainWeaponBase = MainMenu.Instance.Weapons[0];
        _subWeaponBase = MainMenu.Instance.Weapons[1];
        _equipmentWeapon = _mainWeaponBase;
        _weaponAction = _equipmentWeapon.GetComponent<WeaponAction>();
    }

    void Start()
    {
        if (_mainWeaponBase != null && _subWeaponBase != null)
        {
            WeaponChangeMethod();
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("WeaponChange")
            && !Input.GetButton("Attack"))
        {
            WeaponChangeMethod();
        }

        if (!MenuHander.Instance.MenuIsOpen)
        {
            _weaponAction.WeaponAttackMethod(_equipmentWeapon.name);
        }

        if(PlayerContoller.Instance.IsWalled())
        {
            _equipmentWeapon.RendererActive(false);
        }
        else
        {
            _equipmentWeapon.RendererActive(true);
        }
    }
    void WeaponChangeMethod()
    {
        //武器のメインとサブの表示を切り替える

        _weaponSwitch = !_weaponSwitch;
        _mainWeaponBase.RendererActive(_weaponSwitch);
        _subWeaponBase.RendererActive(!_weaponSwitch);

        //メインとサブの武器を切り替える
        if (_weaponSwitch)
        {
            _equipmentWeapon = _mainWeaponBase;
        }
        else
        {
            _equipmentWeapon = _subWeaponBase;
        }
        _weaponAction = _equipmentWeapon.GetComponent<WeaponAction>();
    }
}

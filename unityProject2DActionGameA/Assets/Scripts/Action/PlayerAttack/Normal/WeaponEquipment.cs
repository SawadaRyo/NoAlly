using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipment : SingletonBehaviour<WeaponEquipment>
{
    [SerializeField] string _filePath = "";
    [SerializeField] WeaponBase[] _weapons = default;

    [Tooltip("����؂�ւ�")] bool _weaponSwitch = false;
    [Tooltip("���C������")] WeaponBase _mainWeaponBase = default;
    [Tooltip("�T�u����")] WeaponBase _subWeaponBase = default;
    [Tooltip("�������̕���")] WeaponBase _equipmentWeapon = default;
    [Tooltip("���݂̑���")] ElementType _type = default;
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
        //����̃��C���ƃT�u�̕\����؂�ւ���

        _weaponSwitch = !_weaponSwitch;
        _mainWeaponBase.RendererActive(_weaponSwitch);
        _subWeaponBase.RendererActive(!_weaponSwitch);

        //���C���ƃT�u�̕����؂�ւ���
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

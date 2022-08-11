using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


/// <summary>
/// ����̑������Ǘ�����R���|�[�l���g
/// </summary>

public class WeaponEquipment : SingletonBehaviour<WeaponEquipment>
{
    [SerializeField] WeaponBase[] _weapons = default;

    [Tooltip("����؂�ւ�")] bool _weaponSwitch = false;
    [Tooltip("���C������")] WeaponBase _mainWeaponBase = default;
    [Tooltip("�T�u����")] WeaponBase _subWeaponBase = default;
    [Tooltip("�������̕���")] WeaponBase _equipmentWeapon = default;
    [Tooltip("���݂̑���")] ElementType _type = default;
    WeaponAction _weaponAction = default;
    ObjectPool<Bullet>[] _pool = new ObjectPool<Bullet>[4];

    public WeaponBase EquipmentWeapon { get => _equipmentWeapon; set => _equipmentWeapon = value; }
    public WeaponAction EquipeWeaponAction => _weaponAction;

    private void Awake()
    {
        ChangeWeapon(EquipmentType.MAIN, WeaponName.SWORD);
        ChangeWeapon(EquipmentType.SUB, WeaponName.LANCE);
        _equipmentWeapon = _mainWeaponBase;
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
    public void ChangeWeapon(EquipmentType equipmentType,WeaponName? weaponName)
    {
        if(equipmentType == EquipmentType.MAIN)
        {
            _mainWeaponBase = _weapons[(int)weaponName];
        }
        
        else if(equipmentType == EquipmentType.SUB)
        {
            _subWeaponBase = _weapons[(int)weaponName];
        }
    }
}

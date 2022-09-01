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

    public bool WeaponSwitch => _weaponSwitch;
    public WeaponAction EquipeWeaponAction => _weaponAction;

    private void Awake()
    {
        //TODO:����ݒ��Excel����s����悤�ɂ���
        _mainWeaponBase = _weapons[0];
        _subWeaponBase = _weapons[1];

        ChangeWeapon(EquipmentType.MAIN, WeaponName.SWORD);
        ChangeWeapon(EquipmentType.SUB, WeaponName.LANCE);
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        _weaponSwitch = true;
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
    /// <summary>����̃��C���ƃT�u�̕\����؂�ւ���</summary>
    void SwichWeapon()
    {
        WeaponBase unEquipmentWeapon = default;
        _weaponSwitch = !_weaponSwitch;


        //���C���ƃT�u�̕����؂�ւ���
        if (_weaponSwitch)
        {
            _equipmentWeapon = _mainWeaponBase;
            unEquipmentWeapon = _subWeaponBase;
            //SetEquipment(_mainWeaponBase, _subWeaponBase);
        }
        else
        {
            _equipmentWeapon = _subWeaponBase;
            unEquipmentWeapon = _mainWeaponBase;
            //SetEquipment(_subWeaponBase, _mainWeaponBase);
        }
        SetEquipment(_equipmentWeapon, unEquipmentWeapon);
        _weaponAction = _equipmentWeapon.GetComponent<WeaponAction>();
    }
    /// <summary>���C������E�T�u�����؂�ւ���֐��E�������F���C�����T�u���w��E�ύX����������̖��O</summary>
    /// <param name="equipmentType"></param>
    /// <param name="weaponName"></param>
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

    /// <summary>����̑��������߂�֐�
    /// �E�������F���������镐��
    /// �E�������F���������Ă�������</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="unEquipmentWeapon"></param>
    void SetEquipment(WeaponBase equipmentWeapon, WeaponBase unEquipmentWeapon)
    {
        _equipmentWeapon = equipmentWeapon;
        unEquipmentWeapon.IsActive = false;
        unEquipmentWeapon.RendererActive(false);
        _equipmentWeapon.IsActive = true;
        _equipmentWeapon.RendererActive(true);
    }
}

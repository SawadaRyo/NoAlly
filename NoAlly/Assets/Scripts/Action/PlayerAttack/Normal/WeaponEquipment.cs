using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


/// <summary>
/// ����̑������Ǘ�����R���|�[�l���g
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField] WeaponAction[] _weapons = new WeaponAction[4];

    [Tooltip("���킪�g�p�\�����肷�邽�߂̕ϐ�")]
    bool _available = true;

    [Tooltip("���C������")]
    WeaponAction _mainWeaponBase = null;
    [Tooltip("�T�u����")]
    WeaponAction _subWeaponBase = null;
    [Tooltip("�������̕���")]
    WeaponAction _equipmentWeapon = null;

    //public bool WeaponSwitch => _weaponSwitch;
    public bool Available => _available;
    public WeaponAction[] Weapons => _weapons;
    public WeaponAction EquipeWeapon => _equipmentWeapon;

    public void Init()
    {
        //TODO:����ݒ��Excel����s����悤�ɂ���
        _mainWeaponBase = _weapons[0];
        _subWeaponBase = _weapons[1];

        SetEquipment(_mainWeaponBase, _subWeaponBase);
    }


    void Update()
    {
        //if (!MenuHander.Instance.MenuIsOpen)
        {
            _equipmentWeapon.WeaponAttack();

            if (!Input.GetButton("Attack"))
            {
                var swichFlg = Input.GetButton("SubWeaponSwitch");
                Debug.Log(PlayerAnimationState.Instance.IsAttack);
                SwichWeapon(swichFlg);
            }
        }
    }
    /// <summary>���C������E�T�u����̕\����؂�ւ���֐�</summary>
    void SwichWeapon(bool weaponSwitch)
    {
        WeaponAction unEquipmentWeapon = null;

        if (PlayerAnimationState.Instance.IsAttack) return;

        //���C���ƃT�u�̕����؂�ւ���
        if (!weaponSwitch)
        {
            _equipmentWeapon = _mainWeaponBase;
            unEquipmentWeapon = _subWeaponBase;
        }
        else
        {
            _equipmentWeapon = _subWeaponBase;
            unEquipmentWeapon = _mainWeaponBase;
        }
        SetEquipment(_equipmentWeapon, unEquipmentWeapon);
    }
    /// <summary>���C������E�T�u����̑�����؂�ւ���֐��E�������F���C�����T�u���w��E�ύX����������̖��O</summary>
    /// <param name="equipmentType"></param>
    /// <param name="weaponName"></param>
    public void ChangeWeapon(CommandType equipmentType, WeaponName weaponName)
    {
        _equipmentWeapon.Base.RendererActive(false);
        switch (equipmentType)
        {
            case CommandType.MAIN:
                _mainWeaponBase = _weapons[(int)weaponName];
                break;
            case CommandType.SUB:
                _subWeaponBase = _weapons[(int)weaponName];
                break;
            default:
                break;
        }
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        MainMenu.Instance.DisideElement(MainMenu.Instance.Type);
    }

    /// <summary>_equipmentWeapon�̒��g��ύX����֐�
    /// �E�������F���������镐��
    /// �E�������F���������Ă�������</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="unEquipmentWeapon"></param>
    void SetEquipment(WeaponAction equipmentWeapon, WeaponAction unEquipmentWeapon)
    {
        if (_equipmentWeapon != null)
        {
            _equipmentWeapon.Base.Operated = false;
        }
        _equipmentWeapon = equipmentWeapon;
        _equipmentWeapon.Base.Operated = true;
        _equipmentWeapon.Base.RendererActive(true);
        unEquipmentWeapon.Base.RendererActive(false);
    }

    public void AvailableWeapon(bool available)
    {
        _equipmentWeapon.Base.RendererActive(available);
        _available = available;
    }
}

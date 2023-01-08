using System;
using UnityEngine;
using UniRx;


/// <summary>
/// ���푕����؂�ւ���N���X
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField, Tooltip("����̔z�u���W")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length - 1];

    [Tooltip("����̃v���n�u")]
    GameObject[] _weaponPrefabs = new GameObject[Enum.GetNames(typeof(WeaponType)).Length - 1];
    [Tooltip("���킪�g�p�\�����肷�邽�߂̕ϐ�")]
    bool _available = true;
    [Tooltip("�������̕���")]
    WeaponDateEntity _equipmentWeapon;



    public bool Available => _available;
    public WeaponDateEntity EquipeWeapon => _equipmentWeapon;

    WeaponDateEntity mainWeapon => MainMenu.Instance.Main.Value;
    WeaponDateEntity subWeapon => MainMenu.Instance.Sub.Value;


    void Update()
    {
        if (!MenuHander.Instance.MenuIsOpen && !Input.GetButton("Attack"))
        {
            var swichFlg = Input.GetButton("SubWeaponSwitch");
            Debug.Log(PlayerAnimationState.Instance.IsAttack);
            SwichWeapon(swichFlg);
        }
    }
    public void Initialize()
    {
        //����̃v���n�u�𐶐�
        WeaponDateEntity[] allWeapon = SetWeaponData.Instance.GetAllWeapons();
        for (int index = 0; index < _weaponPrefabs.Length; index++)
        {
            _weaponPrefabs[index] = Instantiate(allWeapon[index].Prefab, _weaponTransform[index]);
            Renderer[] renderers = _weaponPrefabs[index].GetComponentsInChildren<Renderer>();
        }
    }
    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
    /// </summary>
    public void SwichWeapon(bool weaponSwitch)
    {
        WeaponDateEntity unEquipmentWeapon;

        if (PlayerAnimationState.Instance.IsAttack) return;

        //���C���ƃT�u�̕����؂�ւ���
        if (!weaponSwitch)
        {
            _equipmentWeapon = mainWeapon;
            unEquipmentWeapon = subWeapon;
        }
        else
        {
            _equipmentWeapon = subWeapon;
            unEquipmentWeapon = mainWeapon;
        }
        ChangeActiveWeapon(_equipmentWeapon, unEquipmentWeapon);
    }
    /// <param name="equipmentType"></param>
    /// <param name="type"></param>
    /// <summary>_
    /// equipmentWeapon�̕\�����Ǘ�����֐�
    /// </summary>
    /// <param name="equipmentWeapon">���������镐��</param>
    /// <param name="unEquipmentWeapon">���O�܂ő��������Ă�������</param>
    public void ChangeActiveWeapon(WeaponDateEntity equipmentWeapon, WeaponDateEntity unEquipmentWeapon)
    {
        equipmentWeapon.Base.ActiveWeapon(true);
        unEquipmentWeapon.Base.ActiveWeapon(false);
    }

    public void AvailableWeapon(bool available)
    {
        _equipmentWeapon.Base.ActiveWeapon(available);
        _available = available;
    }

}

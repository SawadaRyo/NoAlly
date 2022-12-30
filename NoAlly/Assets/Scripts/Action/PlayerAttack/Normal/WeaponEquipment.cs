using System;
using UnityEngine;
using UniRx;


/// <summary>
/// ����̑������Ǘ�����R���|�[�l���g
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField,Tooltip("����̔z�u���W")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length];

    [Tooltip("���킪�g�p�\�����肷�邽�߂̕ϐ�")]
    bool _available = true;
    [Tooltip("�e����̃v���n�u")]
    WeaponPrefab[] _weaponPrefabs = new WeaponPrefab[Enum.GetNames(typeof(WeaponType)).Length-1];

    [Tooltip("���C������")]
    WeaponDateEntity _mainWeaponBase;
    [Tooltip("�T�u����")]
    WeaponDateEntity _subWeaponBase;
    [Tooltip("�������̕���")]
    WeaponDateEntity _equipmentWeapon;

    //public bool WeaponSwitch => _weaponSwitch;
    public bool Available => _available;
    public WeaponDateEntity EquipeWeapon => _equipmentWeapon;

    void Awake()
    {
        Initialize();
    }
    void Update()
    {
        if (!MenuHander.Instance.MenuIsOpen)
        {
            _equipmentWeapon.Action.WeaponAttack();
            if (!Input.GetButton("Attack"))
            {
                var swichFlg = Input.GetButton("SubWeaponSwitch");
                Debug.Log(PlayerAnimationState.Instance.IsAttack);
                SwichWeapon(swichFlg);
            }
        }
    }
    public void Initialize()
    {
        for(int index = 0;index < _weaponPrefabs.Length;index++)
        {
            _weaponPrefabs[index] = Instantiate(new WeaponPrefab((WeaponType)index), _weaponTransform[index]);
        }
        _mainWeaponBase = SetWeaponData.Instance.GetWeapon(WeaponType.SWORD);
        _subWeaponBase = SetWeaponData.Instance.GetWeapon(WeaponType.LANCE);

        SetEquipment(_mainWeaponBase, _subWeaponBase);
    }
    /// <summary>
    /// ���C������E�T�u����̑������{�^���Ő؂�ւ���֐�
    /// </summary>
    void SwichWeapon(bool weaponSwitch)
    {
        WeaponDateEntity unEquipmentWeapon;

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
    /// <summary>���C������E�T�u����̑��������j���[��ʂ���؂�ւ���֐��E�������F���C�����T�u���w��E�ύX����������̖��O</summary>
    /// <param name="equipmentType"></param>
    /// <param name="type"></param>
    public void ChangeWeapon(CommandType equipmentType, WeaponType type)
    {
        _equipmentWeapon.Base.RendererActive(false);
        switch (equipmentType)
        {
            case CommandType.MAIN:
                _mainWeaponBase = SetWeaponData.Instance.GetWeapon(type);
                break;
            case CommandType.SUB:
                _subWeaponBase = SetWeaponData.Instance.GetWeapon(type);
                break;
            default:
                break;
        }
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        MainMenu.Instance.DisideElement(MainMenu.Instance.Element);
    }
    /// <summary>_equipmentWeapon�̒��g��ύX����֐�
    /// �E�������F���������镐��
    /// �E�������F���������Ă�������</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="unEquipmentWeapon"></param>
    void SetEquipment(WeaponDateEntity equipmentWeapon, WeaponDateEntity unEquipmentWeapon)
    {
        if (_equipmentWeapon.Type == WeaponType.NONE)
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

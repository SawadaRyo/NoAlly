using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ����E�G���`�����g��ύX���邽�߂̃R���|�[�l���g
/// </summary>

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] Button[] _mainWeapons = default;
    [SerializeField] Button[] _subWeapons = default;
    [SerializeField] Button[] _elements = default;

    EquipmentState _main = default;
    EquipmentState _sub = default;


    public event Action<ElementType> DisideElement;

    private void Awake()
    {
        for (int y = 0; y < Enum.GetNames(typeof(WeaponName)).Length; y++)
        {
            int index = y;
            _mainWeapons[y].onClick.AddListener(() => Equipment(EquipmentType.MAIN, (WeaponName)index));
            _subWeapons[y].onClick.AddListener(() => Equipment(EquipmentType.SUB, (WeaponName)index));
            _elements[y].onClick.AddListener(() => DisideElement((ElementType)index));
        }
        _main = new EquipmentState(EquipmentType.MAIN, WeaponName.SWORD);
        _sub = new EquipmentState(EquipmentType.SUB, WeaponName.LANCE);
    }
    /// <summary> �����ő��������؂�ւ���iMain��Sub�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���j</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    void Equipment(EquipmentType type, WeaponName weaponName)
    {
        WeaponName beforeWeapons = default;
        if (type == EquipmentType.MAIN)
        {
           beforeWeapons = _main.Name;
            _main.Name = weaponName;
            if (_sub.Name == _main.Name)
            {
                _sub.Name = beforeWeapons;
            }
        }
        else if (type == EquipmentType.SUB)
        {
            beforeWeapons = _sub.Name;
            _sub.Name = weaponName;
            if (_main.Name == _sub.Name)
            {
                _main.Name = beforeWeapons;
            }
        }

        WeaponEquipment.Instance.ChangeWeapon(_main.Type, _main.Name);
        WeaponEquipment.Instance.ChangeWeapon(_sub.Type, _sub.Name);
    }
}

public struct EquipmentState
{
    public EquipmentType Type;
    public WeaponName Name;

    public EquipmentState(EquipmentType type, WeaponName name)
    {
        Type = type;
        Name = name;
    }
}
public enum EquipmentType
{
    MAIN,
    SUB,
    ElEMENT
}
public enum WeaponName
{
    SWORD = 0,
    LANCE = 1,
    BOW = 2,
    BRASTAR = 3,
}
public enum ElementType
{
    RIGIT = 0,
    FIRE = 1,
    ELEKE = 2,
    FROZEN = 3
}



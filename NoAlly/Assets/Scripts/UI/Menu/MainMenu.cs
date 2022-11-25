using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] Button[] _mainWeapons = null;
    [SerializeField] Button[] _subWeapons = null;
    [SerializeField] Button[] _elements = null;
    [SerializeField] WeaponTable _weaponsData;

    WeaponEquipment _weaponEquipment = null;
    List<IWeapon> _equipmentWeapons = new();
    ReactiveProperty<WeaponDateEntity> _main = new ReactiveProperty<WeaponDateEntity>();
    ReactiveProperty<WeaponDateEntity> _sub = new ReactiveProperty<WeaponDateEntity>();
    ElementType _elementType = default;


    public ElementType Type => _elementType;
    public WeaponTable WeaponsData => _weaponsData;
    public IReadOnlyReactiveProperty<WeaponDateEntity> Main => _main;
    public IReadOnlyReactiveProperty<WeaponDateEntity> Sub => _sub;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        _weaponEquipment = GameObject.FindObjectOfType<WeaponEquipment>();
        int weaponIndexNumber = Enum.GetNames(typeof(WeaponName)).Length;
        for (int y = 0; y < weaponIndexNumber; y++)
        {
            int index = y;
            _mainWeapons[y].onClick.AddListener(() => Equipment(CommandType.MAIN, _weaponsData.WeaponData[index]));
            _subWeapons[y].onClick.AddListener(() => Equipment(CommandType.SUB, _weaponsData.WeaponData[index]));
            _elements[y].onClick.AddListener(() => DisideElement((ElementType)index));
            _equipmentWeapons.Add(_weaponEquipment.Weapons[y].Base);
        }
        _main.Value = _weaponsData.WeaponData[0];
        _sub.Value = _weaponsData.WeaponData[1];
    }
    /// <summary> ここで装備武器を切り替える（MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える）</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDateEntity weaponName)
    {
        WeaponDateEntity beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _main.Value;
            _main.Value = weaponName;
            if (_sub.Value.Name == _main.Value.Name)
            {
                _sub.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _sub.Value;
            _sub.Value = weaponName;
            if (_main.Value.Name == _sub.Value.Name)
            {
                _main.Value = beforeWeapons;
            }
        }

        //WeaponEquipment.Instance.ChangeWeapon(CommandType.MAIN, _main.Name);
        //WeaponEquipment.Instance.ChangeWeapon(CommandType.SUB, _sub.Name);
    }
    public void DisideElement(ElementType element)
    {
        _elementType = element;
        _equipmentWeapons.ToList().ForEach(x => x.WeaponMode(element));
    }

    private void OnDisable()
    {
        _main.Dispose();
        _sub.Dispose();
    }
}

public class MenuCommandButton
{
    bool _isSelected;
    Button _commund;
    WeaponName _weaponName;
    ElementType _elementType;
    CommandType _type;

    public bool IsSelected => _isSelected;
    public Button Command => _commund;
    public WeaponName Name => _weaponName;
    public ElementType ElementType => _elementType;
    public CommandType Type => _type;
    public MenuCommandButton(bool isSelected, Button button, WeaponName name, CommandType type)
    {
        _isSelected = isSelected;
        _commund = button;
        _weaponName = name;
        _type = type;
    }
    public MenuCommandButton(bool isSelected, Button button, ElementType element, CommandType type)
    {
        _isSelected = isSelected;
        _commund = button;
        _elementType = element;
        _type = type;
    }
}
public enum CommandType
{
    MAIN = 0,
    SUB = 1,
    ElEMENT = 2
}
public enum WeaponName
{
    SWORD = 0,
    LANCE = 1,
    BOW = 2,
    BRASTER = 3,
}
public enum ElementType
{
    RIGIT = 0,
    FIRE = 1,
    ELEKE = 2,
    FROZEN = 3
}



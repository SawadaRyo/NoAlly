using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] Button[] _mainWeapons = default;
    [SerializeField] Button[] _subWeapons = default;
    [SerializeField] Button[] _elements = default;

    EquipmentState _main = new EquipmentState();
    EquipmentState _sub = new EquipmentState();
    ElementType _elementType = default;


    public event Action<ElementType> DisideElement;

    private void Start()
    {
        for (int y = 0; y < Enum.GetNames(typeof(WeaponName)).Length; y++)
        {
            _mainWeapons[y].onClick.AddListener(() => Equipment(EquipmentType.MAIN, (WeaponName)y));
            _subWeapons[y].onClick.AddListener(() => Equipment(EquipmentType.SUB, (WeaponName)y));
            _elements[y].onClick.AddListener(() => DisideElement((ElementType)y));
        }
    }
    /// <summary> ここで装備武器を切り替える（MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える）</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    void Equipment(EquipmentType type, WeaponName weaponName)
    {
        WeaponName? beforeWeapons = null;
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
    public WeaponName? Name;
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
    ELEKE = 1,
    FIRE = 2,
    FROZEN = 3
}



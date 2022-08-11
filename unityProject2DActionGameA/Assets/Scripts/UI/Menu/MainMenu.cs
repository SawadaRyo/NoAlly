using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] string _filePath = "";
    [SerializeField] Button[] _mainWeapons = default;
    [SerializeField] Button[] _subWeapons = default;
    [SerializeField] Button[] _elements = default;

    EquipmentState _main = new EquipmentState();
    EquipmentState _sub = new EquipmentState();
    ElementType _elementType = default;

    public EquipmentState Main => _main;
    public EquipmentState Sub => _sub;

    public event Action<ElementType> DisideElement;

    private void Start()
    {
        for (int y = 0; y < Enum.GetNames(typeof(WeaponName)).Length; y++)
        {
            _mainWeapons[y].onClick.AddListener(() => EquipmentWeapon(EquipmentType.MAIN, (WeaponName)y));
            _subWeapons[y].onClick.AddListener(() => EquipmentWeapon(EquipmentType.SUB, (WeaponName)y));
            _elements[y].onClick.AddListener(() => DisideElement((ElementType)y));
        }
    }
    /// <summary> ここで装備武器を切り替える（MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える）</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    void EquipmentWeapon(EquipmentType type, WeaponName weaponName)
    {
        WeaponName? beforeWeapons = null;
        if (type == EquipmentType.MAIN)
        {
            //beforeWeapon = global::WeaponEquipment.Instance.MainWeapon;
            //global::WeaponEquipment.Instance.MainWeapon = weapon;
            //if (global::WeaponEquipment.Instance.MainWeapon == global::WeaponEquipment.Instance.SubWeapon)
            //{
            //    global::WeaponEquipment.Instance.SubWeapon = beforeWeapon;
            //}
            beforeWeapons = _main.WeaponName;
            _main.WeaponName = weaponName;
            if (_sub.WeaponName == _main.WeaponName)
            {
                _sub.WeaponName = beforeWeapons;
            }
        }
        else if (type == EquipmentType.SUB)
        {
            //beforeWeapon = WeaponEquipment.Instance.SubWeapon;
            //WeaponEquipment.Instance.SubWeapon = weapon;
            //if (WeaponEquipment.Instance.MainWeapon == WeaponEquipment.Instance.SubWeapon)
            //{
            //    WeaponEquipment.Instance.MainWeapon = beforeWeapon;
            //}
            beforeWeapons = _sub.WeaponName;
            _sub.WeaponName = weaponName;
            if (_main.WeaponName == _sub.WeaponName)
            {
                _main.WeaponName = beforeWeapons;
            }
        }
    }
}

public struct EquipmentState
{
    public EquipmentType Type;
    public WeaponName? WeaponName;
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



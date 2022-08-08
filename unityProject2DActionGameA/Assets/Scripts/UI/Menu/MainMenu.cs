using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] string _filePath = "";
    [SerializeField] Button[] _mainWeapons = default;
    [SerializeField] Button[] _subWeapons = default;
    [SerializeField] Button[] _elements = default;
    WeaponBase[] _weapons = new WeaponBase[4];
    ElementType _elementType = default;

    public WeaponBase[] Weapons {get => _weapons; set => _weapons = value; }
    public event Action<ElementType> DisideElement;

    private void Start()
    {
        for (int y = 0; y < _weapons.Length; y++)
        {
            var weaponData = _weapons[y];
            _mainWeapons[y].onClick.AddListener(() => WeaponEquipment(weaponData,EquipmentType.MAIN)) ;
            _subWeapons[y].onClick.AddListener(() => WeaponEquipment(weaponData, EquipmentType.SUB)) ;
            _elements[y].onClick.AddListener(() => ElementEquipent((ElementType)y));
        }
    }
    /// <summary> ‚±‚±‚Å‘•”õ•Ší‚ğØ‚è‘Ö‚¦‚éiMain‚ÆSub‚Ì‘•”õ•Ší‚ª“¯‚¶‚¾‚Á‚½ê‡‚»‚ê‚¼‚ê‚Ì‘•”õ•Ší‚ğ“ü‚ê‘Ö‚¦‚éj</summary>
    /// <param name="weapon"></param>
    /// <param name="type"></param>
    void WeaponEquipment(WeaponBase weapon, EquipmentType type)
    {
        WeaponBase beforeWeapon = null;
        if (type == EquipmentType.MAIN)
        {
            beforeWeapon = global::WeaponEquipment.Instance.MainWeapon;
            global::WeaponEquipment.Instance.MainWeapon = weapon;
            if (global::WeaponEquipment.Instance.MainWeapon == global::WeaponEquipment.Instance.SubWeapon)
            {
                global::WeaponEquipment.Instance.SubWeapon = beforeWeapon;
            }
        }
        else if(type == EquipmentType.SUB)
        {
            beforeWeapon = global::WeaponEquipment.Instance.SubWeapon;
            global::WeaponEquipment.Instance.SubWeapon = weapon;
            if (global::WeaponEquipment.Instance.MainWeapon == global::WeaponEquipment.Instance.SubWeapon)
            {
                global::WeaponEquipment.Instance.MainWeapon = beforeWeapon;
            }
        }
    }
    void ElementEquipent(ElementType type)
    {
        DisideElement(type);
    }
}
public enum EquipmentType
{
    MAIN,
    SUB,
    ElEMENT
}
public enum ElementType
{
    RIGIT = 0,
    ELEKE = 1,
    FIRE = 2,
    FROZEN = 3
}



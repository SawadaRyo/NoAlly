using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : SingletonBehaviour<MainMenu>
{
    [SerializeField] string m_filePath = "";
    [SerializeField] Button[] m_mainWeapons = default;
    [SerializeField] Button[] m_subWeapons = default;
    [SerializeField] Button[] m_elements = default;
    WeaponBase[] weapons = new WeaponBase[4];
    ElementType m_elementType = default;

    public event Action<ElementType> DisideElement;

    private void Start()
    {
        weapons = Resources.LoadAll<WeaponBase>(m_filePath);
        for (int i = 0; i < m_mainWeapons.Length; i++)
        {
            m_mainWeapons[i].onClick.AddListener(() => WeaponEquipment(weapons[i],EquipmentType.MAIN)) ;
            m_subWeapons[i].onClick.AddListener(() => WeaponEquipment(weapons[i],EquipmentType.SUB)) ;
            m_elements[i].onClick.AddListener(() => ElementEquipent((ElementType)i));
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



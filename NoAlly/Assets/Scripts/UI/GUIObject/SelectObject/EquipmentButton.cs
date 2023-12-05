using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentButton : WeaponSelect
{
    EquipmentType _commandType;
    WeaponType _weaponType;
    ElementType _elementType;

    public EquipmentType CommandType => _commandType;
    public WeaponType WeaponType => _weaponType;
    public ElementType ElementType => _elementType;

    public EquipmentButton(EquipmentType commandType, WeaponType weaponType)
    {
        _commandType = commandType;
        _weaponType = weaponType;
    }
    public EquipmentButton(EquipmentType commandType, ElementType elementType)
    {
        _commandType = commandType;
        _elementType = elementType;
    }
}

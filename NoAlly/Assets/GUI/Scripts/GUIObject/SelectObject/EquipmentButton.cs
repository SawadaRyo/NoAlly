using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentButton : WeaponSelect
{
    CommandType _commandType;
    WeaponType _weaponType;
    ElementType _elementType;

    public CommandType CommandType => _commandType;
    public WeaponType WeaponType => _weaponType;
    public ElementType ElementType => _elementType;

    public EquipmentButton(CommandType commandType, WeaponType weaponType)
    {
        _commandType = commandType;
        _weaponType = weaponType;
    }
    public EquipmentButton(CommandType commandType, ElementType elementType)
    {
        _commandType = commandType;
        _elementType = elementType;
    }
}

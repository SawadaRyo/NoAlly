using UnityEngine.UI;

public class MenuCommandButton
{
    bool _isSelected;
    Button _commund;
    WeaponType _weaponName;
    ElementType _elementType;
    CommandType _type;

    public bool IsSelected => _isSelected;
    public Button Command => _commund;
    public WeaponType TypeOfWeapon => _weaponName;
    public ElementType TypeOfElement => _elementType;
    public CommandType TypeOfCommand => _type;

    public MenuCommandButton(bool isSelected,Button button)
    {
        _isSelected = isSelected;
        _commund = button;
    }
    public MenuCommandButton(bool isSelected, Button button, WeaponType name, CommandType type)
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


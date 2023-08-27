using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : UIObjectBase
{
    [SerializeField, Header("Ø‚è‘Ö‚í‚é‰æ‘œ")]
    protected Image _switchImage = null;

    [Tooltip("")]
    EquipmentType _type;
    [Tooltip("")]
    WeaponType _weaponName;

    public Sprite WeaponSprite => _switchImage.sprite;
    public EquipmentType Type => _type;
    public WeaponType WeaponName => _weaponName;

    public void SetStatus(EquipmentType type, WeaponType weaponName)
    {
        _type = type;
        _weaponName = weaponName;
    }
}



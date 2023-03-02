using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCommandButton : CommandButton,IWeaponCommand
{
    [SerializeField, Header("����̎��")]
    WeaponType _weaponName;

    public WeaponType TypeOfWeapon => _weaponName;
}

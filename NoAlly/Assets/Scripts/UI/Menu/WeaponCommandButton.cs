using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCommandButton : CommandButton,IWeaponCommand
{
    [SerializeField, Header("•Ší‚ÌŽí—Þ")]
    WeaponType _weaponName;

    public WeaponType TypeOfWeapon => _weaponName;
}

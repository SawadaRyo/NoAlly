using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowWeapon:IWeaponBase
{
    public void InsBullet(IWeaponAction weaponAction);
}

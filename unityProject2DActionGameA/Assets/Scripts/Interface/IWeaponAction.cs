using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAction
{
    public void WeaponChargeAttackMethod(float chrageCount);
    public void WeaponAttackMethod(string weaponName);
}

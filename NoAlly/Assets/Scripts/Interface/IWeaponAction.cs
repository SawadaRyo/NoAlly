using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAction
{
    public void WeaponChargeAttackMethod();
    public void WeaponAttack(WeaponActionType actionType,WeaponType weaponType);
}

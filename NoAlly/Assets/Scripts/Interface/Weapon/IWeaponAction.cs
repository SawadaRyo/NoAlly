using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponAction
{
    public void WeaponAttackMovement(Collider target);
    public void WeaponAttack(Animator playerAnimator, WeaponActionType actionType, WeaponType weaponType);
    public void WeaponChargeAttackMethod();
}

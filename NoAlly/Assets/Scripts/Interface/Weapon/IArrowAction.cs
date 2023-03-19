using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowAction:IWeaponAction
{
    public BulletType WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, ElementType elementType);
}

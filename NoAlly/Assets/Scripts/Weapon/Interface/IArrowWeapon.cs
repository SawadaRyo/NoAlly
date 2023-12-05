using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowWeapon
{
    public void InsBullet();
    public BulletType WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, ElementType elementType);
}

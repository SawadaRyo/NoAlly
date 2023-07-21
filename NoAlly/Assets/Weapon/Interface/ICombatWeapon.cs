using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatWeapon
{
    public float[] CombatChargeAttackMethod(float chrageCount, float[] chargeLevels, float[] weaponPower, ElementType elementType);
}

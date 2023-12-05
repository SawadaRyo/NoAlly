using System;
using UnityEngine;

public class ArrowAction
{
    public BulletType WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, ElementType elementType)
    {
        //ã≠âªíe
        if (chrageCount > chargeLevels[0] && chrageCount <= chargeLevels[1])
        {
            return BulletType.STRENGTHEN;
        }
        //ëÂñC
        else if (chrageCount > chargeLevels[1])
        {
            return BulletType.CANNON;
        }

        //í èÌíe
        return BulletType.NORMAL;
    }
}

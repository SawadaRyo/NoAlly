using System;
using UnityEngine;

public class ArrowAction
{
    public BulletType WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, ElementType elementType)
    {
        //�����e
        if (chrageCount > chargeLevels[0] && chrageCount <= chargeLevels[1])
        {
            return BulletType.STRENGTHEN;
        }
        //��C
        else if (chrageCount > chargeLevels[1])
        {
            return BulletType.CANNON;
        }

        //�ʏ�e
        return BulletType.NORMAL;
    }
}

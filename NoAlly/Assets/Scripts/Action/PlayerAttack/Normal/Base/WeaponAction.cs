using System;
using UnityEngine;

public abstract class WeaponAction : IWeaponAction
{
    [Tooltip("•Ší‚ÌƒvƒŒƒnƒu")]
    protected ObjectBase _weaponPrefab = null;
    [Tooltip("WeaponBase‚ğŠi”[‚·‚é•Ï”")]
    protected IWeaponBase _weaponBase = null;

    public float ChargeCount { get; set; }

    protected float[] ChargePower(float[] weaponPower, ElementType top, float magnification)
    {
        if (magnification < 1)
        {
            magnification = 1;
        }
        weaponPower[(int)ElementType.RIGIT] *= magnification;
        switch (top)
        {
            case ElementType.RIGIT:
                weaponPower[(int)ElementType.RIGIT] *= magnification;
                break;
            case ElementType.FIRE:
                weaponPower[(int)ElementType.FIRE] *= magnification;
                break;
            case ElementType.ELEKE:
                weaponPower[(int)ElementType.ELEKE] *= magnification;
                break;
            case ElementType.FROZEN:
                weaponPower[(int)ElementType.FROZEN] *= magnification;
                break;
        }
        return weaponPower;
    }
}
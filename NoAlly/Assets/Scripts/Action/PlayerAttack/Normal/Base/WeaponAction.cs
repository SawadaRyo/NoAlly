using System;
using UnityEngine;

public class WeaponAction : IWeaponAction
{
    [SerializeField, Header("—­‚ßUŒ‚‘æ1’iŠK")]
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("—­‚ßUŒ‚‘æ2’iŠK")]
    protected float _chargeLevel2 = 3f;

    [Tooltip("•Ší‚ÌƒvƒŒƒnƒu")]
    protected ObjectBase _weaponPrefab = null;
    [Tooltip("WeaponBase‚ðŠi”[‚·‚é•Ï”")]
    protected IWeaponBase _weaponBase = null;

    public float ChargeLevel1 => _chargeLevel1;

    public virtual void WeaponChargeAttackMethod(float chrageCount, float[] weaponPower, ElementType elementType) { }


    
    public virtual void HitMovement(Collider target, IWeaponBase weaponBase)
    {
        //float damage = ChargePower(weaponBase.WeaponPower);
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp) && _weaponBase.Owner != characterHp.Owner)
        {
            characterHp.BehaviorOfHIt(weaponBase.WeaponPower, _weaponBase.ElementType);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj) && _weaponBase.Owner == ObjectOwner.PLAYER)
        {
            hitObj.BehaviorOfHit(_weaponBase, _weaponBase.ElementType);
        }
    }
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
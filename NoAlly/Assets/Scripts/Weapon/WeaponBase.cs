using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase
{
    [Tooltip("•Ší‚ÌƒI[ƒi[")]
    protected ObjectOwner _owner = ObjectOwner.PLAYER;

    [Tooltip("•Ší‚ÌUŒ‚—Í —v‘f1:•Ší‚Ì•¨—UŒ‚—Í,—v‘f2:•Ší‚Ì‰ŠUŒ‚—Í,—v‘f3:•Ší‚Ì—‹UŒ‚—Í,—v‘f4:•Ší‚Ì•XŒ‹UŒ‚—Í")]
    protected float[] _weaponPower = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("‚±‚Ì•Ší‚Ìƒf[ƒ^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("•Ší‚ª•ÏŒ`’†‚©‚Ç‚¤‚©")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("•Ší‚Ìƒ^ƒCƒv")]
    protected WeaponType _weaponType;
    [Tooltip("•Ší‚Ì‘®«")]
    protected ElementType _elementType;

    public float[] WeaponPower => _weaponPower;
    public ElementType ElementType => _elementType;
    public WeaponDeformation Deformated => _isDeformated;
    public ObjectOwner Owner => _owner;

    public WeaponBase(WeaponDataEntity weaponData)
    {
        Debug.Log(weaponData.Type);
        _weaponData = weaponData;
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
    }
    public virtual void WeaponModeToElement(ElementType elementType)
    {
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
        _elementType = elementType;
    }
}



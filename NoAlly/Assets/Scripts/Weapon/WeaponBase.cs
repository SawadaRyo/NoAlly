using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : ObjectBase, IWeapon
{
    [Tooltip("•Ší‚ÌUŒ‚—Í —v‘f1:•Ší‚Ì•¨—UŒ‚—Í,—v‘f2:•Ší‚Ì‰ŠUŒ‚—Í,—v‘f3:•Ší‚Ì—‹UŒ‚—Í,—v‘f4:•Ší‚Ì•XŒ‹UŒ‚—Í")]
    protected float[] _weaponPower = new float[Enum.GetValues(typeof(ElementType)).Length];

    [SerializeField, Header("•Ší‚ÌaŒ‚ƒGƒtƒFƒNƒg")]
    protected ParticleSystem _myParticleSystem = default;
    [SerializeField, Tooltip("•Ší‚ÌƒI[ƒi[")]
    protected ObjectOwner _owner;

    [Tooltip("‚±‚Ì•Ší‚Ìƒf[ƒ^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("•Ší‚ª•ÏŒ`’†‚©‚Ç‚¤‚©")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("•Ší‚Ì‘®«")]
    protected ElementType _type;

    public WeaponDeformation Deformated => _isDeformated;
    public ObjectOwner Owner => _owner;


    public virtual void Initializer(WeaponDataEntity weaponData)
    {
        ActiveObject(_isActive);
        if (_myParticleSystem != null)
        {
            _myParticleSystem.Stop();
        }
        _weaponData = weaponData;
        _weaponPower[(int)ElementType.RIGIT] = weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = weaponData.FrozenPower[(int)_isDeformated];

    }
    public virtual void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp) && _owner != characterHp.Owner)
        {
            characterHp.BehaviorOfHIt(_weaponPower, _type);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj) && _owner == ObjectOwner.PLAYER)
        {
            hitObj.BehaviorOfHit(this, _type);
        }
    }
    public virtual void WeaponMode(ElementType type)
    {
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
        _type = type;
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponAttackMovement(other);
    }
    public void LoopJud(BoolAttack isAttack)
    {
        switch (isAttack)
        {
            case BoolAttack.ATTACKING:
                _myParticleSystem.Play();
                Array.ForEach(this._objectCollider, x => x.enabled = true);
                break;
            default:
                _myParticleSystem.Stop();
                Array.ForEach(this._objectCollider, x => x.enabled = false);
                break;
        }

    }

}



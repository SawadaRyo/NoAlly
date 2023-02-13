using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : ObjectBase, IWeapon
{
    [SerializeField, Header("•Ší‚Ì•¨—UŒ‚—Í")]
    protected float _rigitPower = 5;
    [SerializeField, Header("•Ší‚Ì—‹UŒ‚—Í")]
    protected float _elekePower = 0;
    [SerializeField, Header("•Ší‚Ì‰ŠUŒ‚—Í")]
    protected float _firePower = 0;
    [SerializeField, Header("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    protected float _frozenPower = 0;

    [SerializeField, Header("•Ší‚ÌŽaŒ‚ƒGƒtƒFƒNƒg")]
    protected ParticleSystem _myParticleSystem = default;
    [SerializeField,Tooltip("•Ší‚ÌƒI[ƒi[")]
    protected ObjectOwner _owner;

    [Tooltip("‚±‚Ì•Ší‚Ìƒf[ƒ^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("•Ší‚ª•ÏŒ`’†‚©‚Ç‚¤‚©")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("•Ší‚Ì‘®«")]
    protected ElementType _type;

    public WeaponDeformation Deformated => _isDeformated;
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void SetData(WeaponDataEntity weaponData)
    {
        _weaponData = weaponData;
        _rigitPower = weaponData.RigitPower[(int)_isDeformated];
        _firePower = weaponData.FirePower[(int)_isDeformated];
        _elekePower = weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = weaponData.FrozenPower[(int)_isDeformated];
    }
    public virtual void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp) && _owner != characterHp.Owner)
        {
            characterHp.BehaviorOfHit(this, _type);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj) && _owner == ObjectOwner.PLAYER)
        {
            hitObj.BehaviorOfHit(this, _type);
        }
    }
    public virtual void WeaponMode(ElementType type)
    {
        _rigitPower = _weaponData.RigitPower[(int)_isDeformated];
        _firePower = _weaponData.FirePower[(int)_isDeformated];
        _elekePower = _weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = _weaponData.FrozenPower[(int)_isDeformated];
        _type = type;

    }
    
    void OnTriggerEnter(Collider other)
    {
        WeaponAttackMovement(other);
    }
    public void LoopJud(bool isAttack)
    {
        if (isAttack)
        {
            _myParticleSystem.Play();
        }
        else
        {
            _myParticleSystem.Stop();
        }
        Array.ForEach(this._objectCollider, x => x.enabled = isAttack);
    }

}



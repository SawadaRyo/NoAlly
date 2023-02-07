using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : ObjectVisual, IWeapon
{
    [SerializeField, Header("•Ší‚Ì•¨—UŒ‚—Í")]
    protected float _rigitPower = 5;
    [SerializeField, Header("•Ší‚Ì—‹UŒ‚—Í")]
    protected float _elekePower = 0;
    [SerializeField, Header("•Ší‚Ì‰ŠUŒ‚—Í")]
    protected float _firePower = 0;
    [SerializeField, Header("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    protected float _frozenPower = 0;
    [SerializeField, Header("—­‚ßUŒ‚‘æ1’iŠK")]
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("—­‚ßUŒ‚‘æ2’iŠK")]
    protected float _chargeLevel2 = 3f;
    [SerializeField, Header("•Ší‚ÌUŒ‚”»’èƒŒƒCƒ„[")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Header("•Ší‚ÌƒI[ƒi[")]
    protected HitOwner _owner = HitOwner.Player;
    [SerializeField, Header("•Ší‚ÌUŒ‚”»’è‚Ì’†S“_")]
    Transform _center = default;
    [SerializeField, Header("•Ší‚ÌŽaŒ‚ƒGƒtƒFƒNƒg")]
    protected ParticleSystem _myParticleSystem = default;

    [Tooltip("‚±‚Ì•Ší‚Ìƒf[ƒ^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("•Ší‚ª•ÏŒ`’†‚©‚Ç‚¤‚©")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("")]
    bool _attack = false;
    [Tooltip("•Ší‚Ì‘®«")]
    ElementType _type;

    public WeaponDeformation Deformated => _isDeformated;
    public HitOwner Owner => _owner;
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Initialize(WeaponDataEntity weaponData)
    {
        _weaponData = weaponData;
        _rigitPower = weaponData.RigitPower[(int)_isDeformated];
        _firePower = weaponData.FirePower[(int)_isDeformated];
        _elekePower = weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = weaponData.FrozenPower[(int)_isDeformated];
    }
    public virtual void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp))
        {
            characterHp.BehaviorOfHit(this, _type, _owner);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj))
        {
            hitObj.BehaviorOfHit(_type);
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
    public override void ActiveObject(bool stats)
    {
        base.ActiveObject(stats);
        Array.ForEach(_objectCollider, x => x.enabled = false);
    }
    void OnTriggerEnter(Collider other)
    {
        WeaponAttackMovement(other);
    }
    public void LoopJud(bool isAttack)
    {
        if(isAttack)
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



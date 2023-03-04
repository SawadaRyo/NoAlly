using System;
using UnityEngine;

[RequireComponent(typeof(WeaponBase))]
public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Header("—­‚ßUŒ‚‘æ1’iŠK")]
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("—­‚ßUŒ‚‘æ2’iŠK")]
    protected float _chargeLevel2 = 3f;
    [SerializeField, Header("•Ší‚ÌŽaŒ‚ƒGƒtƒFƒNƒg")]
    protected ParticleSystem _myParticleSystem = default;

    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŽžŠÔ")]
    protected float _chrageCount = 0;
    [Tooltip("•Ší‚ÌƒvƒŒƒnƒu")]
    protected ObjectBase _weaponPrefab = null;
    [Tooltip("WeaponBase‚ðŠi”[‚·‚é•Ï”")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationState‚ðŠi”[‚·‚é•Ï”")]
    PlayerAnimationState _state = null;

    public float ChargeLevel1 => _chargeLevel1;
    public float ChargeLevel2 => _chargeLevel2;

    public virtual void WeaponChargeAttackMethod() { }

    public virtual void Initialize(WeaponBase weaponBase)
    {
        //_weaponPrefab.ActiveObject(false);
        if (_myParticleSystem != null)
        {
            _myParticleSystem.Stop();
        }
        _state = PlayerAnimationState.Instance;
        _weaponBase = weaponBase;
    }

    public void WeaponAttack(Animator playerAnimator, WeaponActionType actionType, WeaponType weaponType)
    {
        if (_state.AbleInput)
        {
            switch (actionType)
            {
                case WeaponActionType.Attack:
                    playerAnimator.SetTrigger("AttackTrigger");
                    playerAnimator.SetInteger("WeaponType", (int)weaponType);
                    break;
                case WeaponActionType.Charging:
                    _chrageCount += Time.deltaTime;
                    playerAnimator.SetBool("Charge", true);
                    break;
                case WeaponActionType.ChargeAttack:
                    if (_chrageCount >= _chargeLevel1)
                    {
                        playerAnimator.SetTrigger("ChargeAttackTrigger");
                    }
                    playerAnimator.SetBool("Charge", false);
                    ResetValue();
                    break;
                default:
                    break;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        WeaponAttackMovement(other);
    }

    public virtual void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp) && _weaponBase.Owner != characterHp.Owner)
        {
            characterHp.BehaviorOfHIt(_weaponBase.WeaponPower, _weaponBase.Type);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj) && _weaponBase.Owner == ObjectOwner.PLAYER)
        {
            hitObj.BehaviorOfHit(_weaponBase, _weaponBase.Type);
        }
    }
    public void LoopJud(BoolAttack isAttack)
    {
        switch (isAttack)
        {
            case BoolAttack.ATTACKING:
                _myParticleSystem.Play();
                _weaponPrefab.ActiveCollider(true);
                break;
            default:
                _myParticleSystem.Stop();
                _weaponPrefab.ActiveCollider(false);
                break;
        }
    }
    public virtual void ResetValue()
    {
        _chrageCount = 0;
    }
}

public enum WeaponActionType
{
    None,
    Attack,
    Charging,
    ChargeAttack,
}
using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ1’iŠK")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ2’iŠK")] protected float _chargeLevel2 = 3f;

    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŽžŠÔ")]
    protected float _chrageCount = 0;
    [Tooltip("•Ší–¼")]
    protected string _weaponName;
    [Tooltip("Player‚ÌAnimator‚ðŠi”[‚·‚é•Ï”")]
    protected Animator _animator = null;
    [Tooltip("WeaponBase‚ðŠi”[‚·‚é•Ï”")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationState‚ðŠi”[‚·‚é•Ï”")]
    PlayerAnimationState _state = null;
    [Tooltip("WeaponEquipment‚ðŠi”[‚·‚é•Ï”")]
    WeaponEquipment _weaponEquipment = null;

    public WeaponBase Base => _weaponBase;

    public virtual void WeaponChargeAttackMethod() { }
    public virtual void Enable() { }

    public void Initialize()
    {
        Enable();
        _weaponName = this.gameObject.name;
        _weaponEquipment = GetComponentInParent<WeaponEquipment>();
        _state = PlayerAnimationState.Instance;
        _animator = GetComponentInParent<PlayerContoller>().GetComponent<Animator>();
        _weaponBase = GetComponent<WeaponBase>();
    }

    public void WeaponAttack(WeaponActionType actionType,WeaponType weaponType)
    {
        if (_state.AbleInput && _weaponEquipment.Available)
        {
            switch(actionType)
            {
                case WeaponActionType.Attack:
                    //_animator.SetTrigger(_weaponName);
                    _animator.SetInteger("Attack",(int)weaponType);
                    break;
                case WeaponActionType.Charging:
                    _chrageCount += Time.deltaTime;
                    break;
                case WeaponActionType.ChargeAttack:
                    _animator.SetBool("Charge", true);
                    break;
                default:
                    break;
            }
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
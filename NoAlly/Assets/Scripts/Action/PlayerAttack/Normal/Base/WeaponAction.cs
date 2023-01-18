using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Header("—­‚ßUŒ‚‘æ1’iŠK")] 
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("—­‚ßUŒ‚‘æ2’iŠK")] 
    protected float _chargeLevel2 = 3f;

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

    public float ChargeLevel1 => _chargeLevel1;
    public float ChargeLevel2 => _chargeLevel2;
    public WeaponBase Base => _weaponBase;

    public virtual void WeaponChargeAttackMethod() { }

    public virtual void Initialize(PlayerContoller player,WeaponBase weaponBase)
    {
        _weaponName = this.gameObject.name;
        _state = PlayerAnimationState.Instance;
        _animator = player.PlayerAnimator;
        _weaponBase = weaponBase;
    }

    public void WeaponAttack(WeaponActionType actionType,WeaponType weaponType)
    {
        if (_state.AbleInput)
        {
            switch(actionType)
            {
                case WeaponActionType.Attack:
                    _animator.SetTrigger("AttackTrigger");
                    _animator.SetInteger("WeaponType",(int)weaponType);
                    break;
                case WeaponActionType.Charging:
                    _chrageCount += Time.deltaTime;
                    _animator.SetBool("Charge", true);
                    break;
                case WeaponActionType.ChargeAttack:
                    _animator.SetBool("Charge", false);
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
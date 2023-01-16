using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Header("���ߍU����1�i�K")] 
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("���ߍU����2�i�K")] 
    protected float _chargeLevel2 = 3f;

    [Tooltip("���ߍU���̗��ߎ���")]
    protected float _chrageCount = 0;
    [Tooltip("���햼")]
    protected string _weaponName;
    [Tooltip("Player��Animator���i�[����ϐ�")]
    protected Animator _animator = null;
    [Tooltip("WeaponBase���i�[����ϐ�")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationState���i�[����ϐ�")]
    PlayerAnimationState _state = null;

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
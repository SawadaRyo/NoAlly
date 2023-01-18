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
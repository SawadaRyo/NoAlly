using System;
using UnityEngine;

public class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ1’iŠK")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ2’iŠK")] protected float _chargeLevel2 = 3f;

    [Tooltip("•ŠíƒRƒ“ƒ|[ƒlƒ“ƒg‚Ì‰Šú‰»")]
    bool _unStored = true;
    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŠÔ")]
    protected float _chrageCount = 0;
    [Tooltip("•Ší–¼")]
    protected string _weaponName;
    [Tooltip("Player‚ÌAnimator‚ğŠi”[‚·‚é•Ï”")]
    protected Animator _animator = null;
    [Tooltip("WeaponBase‚ğŠi”[‚·‚é•Ï”")]
    protected WeaponBase _weaponBase = null;
    [Tooltip("PlayerAnimationState‚ğŠi”[‚·‚é•Ï”")]
    PlayerAnimationState _state = null;
    [Tooltip("WeaponEquipment‚ğŠi”[‚·‚é•Ï”")]
    WeaponEquipment _weaponEquipment = null;

    public WeaponBase Base => _weaponBase;

    public virtual void WeaponChargeAttackMethod() { }
    public virtual void Enable() { }
    public virtual void ResetValue()
    {
        _chrageCount = 0;
    }

    void OnEnable()
    {
        if (_unStored)
        {
            Enable();
            _weaponName = this.gameObject.name;
            _weaponEquipment = GetComponentInParent<WeaponEquipment>();
            _state = PlayerAnimationState.Instance;
            _animator = GetComponentInParent<PlayerContoller>().GetComponent<Animator>();
            _weaponBase = GetComponent<WeaponBase>();
            _unStored = false;
        }
    }

    public void WeaponAttack()
    {
        if (_state.AbleInput && _weaponEquipment.Available)
        {
            ////’ÊíUŒ‚‚Ìˆ—
            if (Input.GetButtonDown("Attack"))
            {
                _animator.SetTrigger(_weaponName);
            }
            //—­‚ßUŒ‚‚Ìˆ—(‹|–î‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚à‚±‚Ìˆ—j
            if (Input.GetButton("Attack") && _chrageCount < _chargeLevel2)
            {
                _chrageCount += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Attack"))
            {
                //if(_chrageCount > 0f)
                //{
                //    WeaponChargeAttackMethod(_chrageCount);
                //}
            }

            _animator.SetBool("Charge", Input.GetButton("Attack"));
        }
    }
}

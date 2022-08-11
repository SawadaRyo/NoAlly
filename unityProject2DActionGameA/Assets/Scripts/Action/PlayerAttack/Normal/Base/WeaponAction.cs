using System;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ1’iŠK")] protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ2’iŠK")] protected float _chargeLevel2 = 3f;

    [Tooltip("")] 
    bool _unStored = true;
    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŠÔ")] 
    protected float _chrageCount = 0;
    [Tooltip("•Ší–¼")] 
    protected string _weaponName;
    [Tooltip("Player‚ÌAnimator‚ğŠi”[‚·‚é•Ï”")]
    protected Animator _animator = default;
    [Tooltip("WeaponBase‚ğŠi”[‚·‚é•Ï”")] 
    protected WeaponBase _weaponBase = default;
    [Tooltip("PlayerAnimationState‚ğŠi”[‚·‚é•Ï”")]
    PlayerAnimationState _state;

    public abstract void WeaponChargeAttackMethod(float chrageCount);
    public virtual void Enable() { }

    void OnEnable()
    {
        if(_unStored)
        {
            Enable();
            _state = PlayerAnimationState.Instance;
            _animator = PlayerContoller.Instance.GetComponent<Animator>();
            _weaponBase = GetComponent<WeaponBase>();
            _unStored = false;
        }
    }
   
    public void WeaponAttack(string weaponName)
    {
        if (!_state.AbleInput) return;

        ////’ÊíUŒ‚‚Ìˆ—
        if (Input.GetButtonDown("Attack"))
        {
            _animator.SetTrigger(weaponName);
        }

        //—­‚ßUŒ‚‚Ìˆ—(‹|–î‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚à‚±‚Ìˆ—j
        if (Input.GetButton("Attack") && _chrageCount < _chargeLevel2)
        {
            _chrageCount += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if(_chrageCount > 0f)
            {
                WeaponChargeAttackMethod(_chrageCount);
            }
            _chrageCount = 0f;
        }

        _animator.SetBool("Charge", Input.GetButton("Attack"));
    }

    
}

using System;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ1’iŠK")] protected float m_chargeLevel1 = 1f;
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ2’iŠK")] protected float m_chargeLevel2 = 1.5f;
    [SerializeField, Tooltip("—­‚ßUŒ‚‘æ3’iŠK")] protected float m_chargeLevel3 = 3f;

    [Tooltip("")] 
    bool m_attacked = false;
    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŠÔ")] 
    protected float m_chrageCount = 0;
    [Tooltip("•Ší–¼")] 
    protected string m_weaponName;
    [Tooltip("Player‚ÌAnimator‚ğŠi”[‚·‚é•Ï”")]
    protected Animator m_animator = default;
    [Tooltip("WeaponBase‚ğŠi”[‚·‚é•Ï”")] 
    protected WeaponBase m_weaponBase = default;
    [Tooltip("PlayerAnimationState‚ğŠi”[‚·‚é•Ï”")]
    PlayerAnimationState m_state;

    public bool Attacked => m_attacked;
    public abstract void WeaponChargeAttackMethod(float chrageCount);

    public virtual void Start()
    {
        m_state = PlayerAnimationState.Instance;
        m_weaponName = WeaponEquipment.Instance.EquipmentWeapon.name;
        m_animator = PlayerContoller.Instance.GetComponent<Animator>();
        m_weaponBase = GetComponent<WeaponBase>();
    }
   
    public void WeaponAttackMethod(string weaponName)
    {
        if (!m_state.AbleInput) return;

        ////’ÊíUŒ‚‚Ìˆ—
        if (Input.GetButtonDown("Attack"))
        {
            m_animator.SetTrigger(weaponName);
        }

        //—­‚ßUŒ‚‚Ìˆ—(‹|–î‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚à‚±‚Ìˆ—j
        if (Input.GetButton("Attack") && m_chrageCount < m_chargeLevel3)
        {
            m_chrageCount += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if(m_chrageCount > m_chargeLevel1)
            {
                WeaponChargeAttackMethod(m_chrageCount);
            }
            m_chrageCount = 0f;
        }

        m_animator.SetBool("Charge", Input.GetButton("Attack"));
    }

    
}

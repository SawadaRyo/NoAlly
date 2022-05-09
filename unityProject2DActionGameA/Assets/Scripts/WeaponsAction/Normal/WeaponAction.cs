using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    [SerializeField, Tooltip("—­‚ßUŒ‚‚Ì—­‚ßƒJƒEƒ“ƒ^[")] int m_chrageAttackCounter = 1800;
    [SerializeField, Tooltip("WeaponChanger‚ğŠi”[‚·‚é•Ï”")] WeaponChanger m_weaponChanger = default;
    [SerializeField, Tooltip("Animator‚ğŠi”[‚·‚é•Ï”")] Animator m_animator = default;

    [Tooltip("—­‚ßUŒ‚‚Ì—­‚ßŠÔ")] protected int m_chrageAttackCount = 0;
    [Tooltip("")] bool m_attacked = false;

    public bool Attacked { get => m_attacked; set => m_attacked = value; }

    public virtual void IsStart()
    {
        //StartŠÖ”‚Ås‚¢‚½‚¢ˆ—‚Í‚±‚±‚É‘‚­
    }

    public virtual void WeaponChargeAttackMethod()
    {
       //•Ší‚²‚Æ‚Ì—­‚ßUŒ‚‚Ìˆ—‚ğ‚±‚±‚É‘‚­
    }
    void WeaponAttackMethod()
    {
        //’ÊíUŒ‚‚Ìˆ—
        if (Input.GetButtonDown("Attack"))
        {
            m_animator.SetTrigger(m_weaponChanger.EquipmentWeapon.name + "Attack");
        }

        //—­‚ßUŒ‚‚Ìˆ—(‹|–î‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚à‚±‚Ìˆ—j
        if (Input.GetButton("Attack") && m_chrageAttackCount < m_chrageAttackCounter)
        {
            m_chrageAttackCount++;
        }
        if (Input.GetButtonUp("Attack"))
        {
            //m_bow.BulletInstance(m_chrageAttackCount);
            if (m_chrageAttackCount > 0)
            {
                m_chrageAttackCount = 0;
            }
        }
        m_animator.SetBool("Charge", Input.GetButton("Attack"));
    }
    void Start()
    {
        IsStart();
    }

    void Update()
    {
        WeaponAttackMethod();
        WeaponChargeAttackMethod();
    }

    void AttackJud()
    {
        m_attacked = !m_attacked;
    }
}

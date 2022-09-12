using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrastarWeapon : CombatWeapon
{
    bool m_isOpen = false;
    float m_normalRigit = 0f;
    float m_normalEleke = 0f;
    float m_powerUpRigit = 3.5f;
    float m_powerUpEleke = 5f;
    Animator m_weaponAnimator = default;
    public override void WeaponMode(ElementType type)
    {
        base.WeaponMode(type);
        if(type == ElementType.ELEKE)
        {
            _rigitPower = m_powerUpRigit;
            _elekePower = m_powerUpEleke;
            m_isOpen = true;
        }
        else
        {
            _rigitPower = m_normalRigit;
            _elekePower = m_normalEleke;
            m_isOpen = false;
        }
        m_weaponAnimator.SetBool("IsOpen",m_isOpen);
    }
    public override void Awake()
    {
        base.Awake();

        m_weaponAnimator = GetComponent<Animator>();
        m_normalRigit = _rigitPower;
        m_normalEleke = _elekePower;
    }

    private void OnEnable()
    {
        MainMenu.Instance.DisideElement += this.WeaponMode;
    }

    private void OnDisable()
    {
        //MainMenu.Instance.DisideElement -= this.WeaponMode;
    }
}

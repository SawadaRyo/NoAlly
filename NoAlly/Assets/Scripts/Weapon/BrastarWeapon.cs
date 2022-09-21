using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrastarWeapon : CombatWeapon
{
    Collider[] _hitJudgeCollideres;
    bool _isOpen = false;
    float _normalRigit = 0f;
    float _normalEleke = 0f;
    float _powerUpRigit = 3.5f;
    float _powerUpEleke = 5f;
    Animator _weaponAnimator = default;

    public override void Awake()
    {
        base.Awake();

        _weaponAnimator = GetComponent<Animator>();
        _normalRigit = _rigitPower;
        _normalEleke = _elekePower;
    }
    private void OnEnable()
    {
        //MainMenu.Instance.DisideElement += this.WeaponMode;
    }
    private void OnDisable()
    {
        //MainMenu.Instance.DisideElement -= this.WeaponMode;
    }
    public override void WeaponMode(ElementType type)
    {
        base.WeaponMode(type);
        if(type == ElementType.ELEKE)
        {
            _rigitPower = _powerUpRigit;
            _elekePower = _powerUpEleke;
            _isOpen = true;
        }
        else
        {
            _rigitPower = _normalRigit;
            _elekePower = _normalEleke;
            _isOpen = false;
        }
        _weaponAnimator.SetBool("IsOpen",_isOpen);
    }
}

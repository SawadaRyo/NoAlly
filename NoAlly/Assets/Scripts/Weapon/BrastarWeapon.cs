using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrastarWeapon : CombatWeapon
{
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
        _normalHarfExtents = new Vector3(0.25f, 1.2f, 0.1f);
        _pawerUpHarfExtents = new Vector3(0.4f, 1.7f, 0.1f);
    }
    public override void WeaponMode(ElementType type)
    {
        base.WeaponMode(type);
        //if(type == ElementType.ELEKE)
        //{
        //    _rigitPower = _powerUpRigit;
        //    _elekePower = _powerUpEleke;
        //    _isOpen = true;
        //}
        //else
        //{
        //    _rigitPower = _normalRigit;
        //    _elekePower = _normalEleke;
        //    _isOpen = false;
        //}

        switch(type)
        {
            case ElementType.ELEKE:
                _harfExtents = _pawerUpHarfExtents;
                _rigitPower = _powerUpRigit;
                _elekePower = _powerUpEleke;
                _isOpen = true;
                break;
            default:
                _harfExtents = _pawerUpHarfExtents;
                _rigitPower = _normalRigit;
                _elekePower = _normalEleke;
                _isOpen = false;
                break;

        }
        _weaponAnimator.SetBool("IsOpen",_isOpen);
    }
    void OnEnable()
    {
        //MainMenu.Instance.DisideElement += this.WeaponMode;
    }
    void OnDisable()
    {
        //MainMenu.Instance.DisideElement -= this.WeaponMode;
    }
}

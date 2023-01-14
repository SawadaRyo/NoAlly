using System;
using UnityEngine;
using UniRx;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [Tooltip("�������Ă��镐��")] 
    WeaponDataEntity _targetweapon;
    [Tooltip("�v���C���[�̓���")]
    WeaponActionType _actionType;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponDataEntity TargetWeapon { get => _targetweapon; set => _targetweapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwichWeapon; 

    // Update is called once per frame
    void Update()
    {
        if (!MenuHander.Instance.MenuIsOpen && !Input.GetButton("Attack"))
        {
            _isSwichWeapon.Value = Input.GetButton("SubWeaponSwitch");
        }
        WeaponAttack();
    }

    

    void WeaponAttack()
    {
        ////�ʏ�U���̏���
        if (Input.GetButtonDown("Attack"))
        {
            _actionType = WeaponActionType.Attack;
        }
        //���ߍU���̏���(�|��̃A�j���[�V���������̏����j
        else if (Input.GetButton("Attack"))
        {
            _actionType = WeaponActionType.Charging;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            _actionType = WeaponActionType.ChargeAttack;
        }
        else
        {
            _actionType = WeaponActionType.None;
        }
        _targetweapon.Action.WeaponAttack(_actionType, _targetweapon.Type);
    }
}






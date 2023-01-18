using System;
using UnityEngine;
using UniRx;

/// <summary>
/// ����̃��[�V����������Ȃǂ̕���ɂ܂�鏈�����s���N���X
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [Tooltip("�������Ă��镐��")]
    WeaponDatas _targetweapon;
    [Tooltip("�v���C���[�̓���")]
    WeaponActionType _actionType;
    float time = 0;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponDatas TargetWeapon { get => _targetweapon; set => _targetweapon = value; }
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
            time += Time.deltaTime;
            if (time > _targetweapon.Action.ChargeLevel1 / 20)
            {
                _actionType = WeaponActionType.Charging;
            }
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if(time > _targetweapon.Action.ChargeLevel1/20)
            {
                _actionType = WeaponActionType.ChargeAttack;
            }
            time = 0;
        }
        _targetweapon.Action.WeaponAttack(_actionType, _targetweapon.Type);
        _actionType = WeaponActionType.None;
    }
}






using System;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [Tooltip("装備している武器")] 
    WeaponDataEntity _targetweapon;
    [Tooltip("プレイヤーの入力")]
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
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            _actionType = WeaponActionType.Attack;
        }
        //溜め攻撃の処理(弓矢のアニメーションもこの処理）
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






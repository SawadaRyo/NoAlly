using System;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのアニメーター")]
    Animator _playerAnimator = null;

    [Tooltip("装備している武器")]
    WeaponDatas _targetWeapon;
    [Tooltip("プレイヤーの入力")]
    WeaponActionType _actionType;
    float time = 0;

    BoolReactiveProperty _isSwichWeapon = new BoolReactiveProperty();

    public WeaponDatas TargetWeapon { get => _targetWeapon; set => _targetWeapon = value; }
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwichWeapon;

    // Update is called once per frame
    void Update()
    {
        if (!WeaponMenuHander.Instance.MenuIsOpen && !Input.GetButton("Attack"))
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
            time += Time.deltaTime;
            if (time > _targetWeapon.Action.ChargeLevel1 / 20)
            {
                _actionType = WeaponActionType.Charging;
            }
        }
        else if (Input.GetButtonUp("Attack"))
        {
            if(time > _targetWeapon.Action.ChargeLevel1/20)
            {
                _actionType = WeaponActionType.ChargeAttack;
            }
            time = 0;
        }
        _targetWeapon.Action.WeaponAttack(_playerAnimator,_actionType, _targetWeapon.Type);
        _actionType = WeaponActionType.None;
    }

    public void WeaponMode(WeaponType weaponType,ElementType elementType)
    {

    }
}






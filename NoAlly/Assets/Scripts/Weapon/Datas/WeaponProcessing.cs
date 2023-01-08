using System;
using UnityEngine;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Header("武器のスクリプタブルオブジェクト")]
    WeaponScriptableObjects _weaponDatas = null;
    [SerializeField, Header("プレイヤーの武器装備クラス")]
    WeaponEquipment _weaponEquipment = null;

    [Tooltip("プレイヤーの入力")]
    WeaponActionType _actionType;
    void Awake()
    {
        if (SetWeaponData.Instance.WeaponDatas == null)
        {
            _weaponEquipment.Initialize();
            SetWeaponData.Instance.WeaponDatas = _weaponDatas;
            SetWeaponData.Instance.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
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
        _weaponEquipment.EquipeWeapon.Action.WeaponAttack(_actionType,_weaponEquipment.EquipeWeapon.Type);
    }
}






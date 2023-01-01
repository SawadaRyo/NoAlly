using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器のモーションや可視化などの武器にまつわる処理を行うクラス
/// </summary>
public class WeaponProcessing : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの武器装備クラス")]
    WeaponEquipment _weaponEquipment = null;

    [Tooltip("プレイヤーの入力")]
    WeaponActionType _actionType;
    // Start is called before the first frame update
    void Start()
    {
        _weaponEquipment.Initialize();
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
        if (Input.GetButton("Attack"))
        {
            _actionType = WeaponActionType.Charging;
        }
        else if (Input.GetButtonUp("Attack"))
        {
            _actionType = WeaponActionType.ChargeAttack;
        }
        _weaponEquipment.EquipeWeapon.Action.WeaponAttack(_actionType,_weaponEquipment.EquipeWeapon.Type);
        _actionType = WeaponActionType.None;
    }
}





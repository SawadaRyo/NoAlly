using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


/// <summary>
/// 武器の装備を管理するコンポーネント
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField] WeaponAction[] _weapons = new WeaponAction[4];

    [Tooltip("武器が使用可能か判定するための変数")]
    bool _available = true;

    [Tooltip("メイン武器")]
    WeaponAction _mainWeaponBase = null;
    [Tooltip("サブ武器")]
    WeaponAction _subWeaponBase = null;
    [Tooltip("装備中の武器")]
    WeaponAction _equipmentWeapon = null;

    //public bool WeaponSwitch => _weaponSwitch;
    public bool Available => _available;
    public WeaponAction[] Weapons => _weapons;
    public WeaponAction EquipeWeapon => _equipmentWeapon;

    public void Init()
    {
        //TODO:武器設定をExcelから行えるようにする
        _mainWeaponBase = _weapons[0];
        _subWeaponBase = _weapons[1];

        SetEquipment(_mainWeaponBase, _subWeaponBase);
    }


    void Update()
    {
        //if (!MenuHander.Instance.MenuIsOpen)
        {
            _equipmentWeapon.WeaponAttack();

            if (!Input.GetButton("Attack"))
            {
                var swichFlg = Input.GetButton("SubWeaponSwitch");
                Debug.Log(PlayerAnimationState.Instance.IsAttack);
                SwichWeapon(swichFlg);
            }
        }
    }
    /// <summary>メイン武器・サブ武器の表示を切り替える関数</summary>
    void SwichWeapon(bool weaponSwitch)
    {
        WeaponAction unEquipmentWeapon = null;

        if (PlayerAnimationState.Instance.IsAttack) return;

        //メインとサブの武器を切り替える
        if (!weaponSwitch)
        {
            _equipmentWeapon = _mainWeaponBase;
            unEquipmentWeapon = _subWeaponBase;
        }
        else
        {
            _equipmentWeapon = _subWeaponBase;
            unEquipmentWeapon = _mainWeaponBase;
        }
        SetEquipment(_equipmentWeapon, unEquipmentWeapon);
    }
    /// <summary>メイン武器・サブ武器の装備を切り替える関数・第一引数：メインかサブか指定・変更したい武器の名前</summary>
    /// <param name="equipmentType"></param>
    /// <param name="weaponName"></param>
    public void ChangeWeapon(CommandType equipmentType, WeaponName weaponName)
    {
        _equipmentWeapon.Base.RendererActive(false);
        switch (equipmentType)
        {
            case CommandType.MAIN:
                _mainWeaponBase = _weapons[(int)weaponName];
                break;
            case CommandType.SUB:
                _subWeaponBase = _weapons[(int)weaponName];
                break;
            default:
                break;
        }
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        MainMenu.Instance.DisideElement(MainMenu.Instance.Type);
    }

    /// <summary>_equipmentWeaponの中身を変更する関数
    /// ・第一引数：装備させる武器
    /// ・第二引数：装備させていた武器</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="unEquipmentWeapon"></param>
    void SetEquipment(WeaponAction equipmentWeapon, WeaponAction unEquipmentWeapon)
    {
        if (_equipmentWeapon != null)
        {
            _equipmentWeapon.Base.Operated = false;
        }
        _equipmentWeapon = equipmentWeapon;
        _equipmentWeapon.Base.Operated = true;
        _equipmentWeapon.Base.RendererActive(true);
        unEquipmentWeapon.Base.RendererActive(false);
    }

    public void AvailableWeapon(bool available)
    {
        _equipmentWeapon.Base.RendererActive(available);
        _available = available;
    }
}

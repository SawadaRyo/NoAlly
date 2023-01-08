using System;
using UnityEngine;
using UniRx;


/// <summary>
/// 武器装備を切り替えるクラス
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField, Tooltip("武器の配置座標")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length - 1];

    [Tooltip("武器のプレハブ")]
    GameObject[] _weaponPrefabs = new GameObject[Enum.GetNames(typeof(WeaponType)).Length - 1];
    [Tooltip("武器が使用可能か判定するための変数")]
    bool _available = true;
    [Tooltip("装備中の武器")]
    WeaponDateEntity _equipmentWeapon;



    public bool Available => _available;
    public WeaponDateEntity EquipeWeapon => _equipmentWeapon;

    WeaponDateEntity mainWeapon => MainMenu.Instance.Main.Value;
    WeaponDateEntity subWeapon => MainMenu.Instance.Sub.Value;


    void Update()
    {
        if (!MenuHander.Instance.MenuIsOpen && !Input.GetButton("Attack"))
        {
            var swichFlg = Input.GetButton("SubWeaponSwitch");
            Debug.Log(PlayerAnimationState.Instance.IsAttack);
            SwichWeapon(swichFlg);
        }
    }
    public void Initialize()
    {
        //武器のプレハブを生成
        WeaponDateEntity[] allWeapon = SetWeaponData.Instance.GetAllWeapons();
        for (int index = 0; index < _weaponPrefabs.Length; index++)
        {
            _weaponPrefabs[index] = Instantiate(allWeapon[index].Prefab, _weaponTransform[index]);
            Renderer[] renderers = _weaponPrefabs[index].GetComponentsInChildren<Renderer>();
        }
    }
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    public void SwichWeapon(bool weaponSwitch)
    {
        WeaponDateEntity unEquipmentWeapon;

        if (PlayerAnimationState.Instance.IsAttack) return;

        //メインとサブの武器を切り替える
        if (!weaponSwitch)
        {
            _equipmentWeapon = mainWeapon;
            unEquipmentWeapon = subWeapon;
        }
        else
        {
            _equipmentWeapon = subWeapon;
            unEquipmentWeapon = mainWeapon;
        }
        ChangeActiveWeapon(_equipmentWeapon, unEquipmentWeapon);
    }
    /// <param name="equipmentType"></param>
    /// <param name="type"></param>
    /// <summary>_
    /// equipmentWeaponの表示を管理する関数
    /// </summary>
    /// <param name="equipmentWeapon">装備させる武器</param>
    /// <param name="unEquipmentWeapon">直前まで装備させていた武器</param>
    public void ChangeActiveWeapon(WeaponDateEntity equipmentWeapon, WeaponDateEntity unEquipmentWeapon)
    {
        equipmentWeapon.Base.ActiveWeapon(true);
        unEquipmentWeapon.Base.ActiveWeapon(false);
    }

    public void AvailableWeapon(bool available)
    {
        _equipmentWeapon.Base.ActiveWeapon(available);
        _available = available;
    }

}

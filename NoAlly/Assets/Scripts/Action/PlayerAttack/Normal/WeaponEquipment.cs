using System;
using UnityEngine;
using UniRx;


/// <summary>
/// 武器の装備を管理するコンポーネント
/// </summary>


public class WeaponEquipment : MonoBehaviour
{
    [SerializeField,Tooltip("武器の配置座標")]
    Transform[] _weaponTransform = new Transform[Enum.GetNames(typeof(WeaponType)).Length];

    [Tooltip("武器が使用可能か判定するための変数")]
    bool _available = true;
    [Tooltip("各武器のプレハブ")]
    WeaponPrefab[] _weaponPrefabs = new WeaponPrefab[Enum.GetNames(typeof(WeaponType)).Length-1];

    [Tooltip("メイン武器")]
    WeaponDateEntity _mainWeaponBase;
    [Tooltip("サブ武器")]
    WeaponDateEntity _subWeaponBase;
    [Tooltip("装備中の武器")]
    WeaponDateEntity _equipmentWeapon;

    //public bool WeaponSwitch => _weaponSwitch;
    public bool Available => _available;
    public WeaponDateEntity EquipeWeapon => _equipmentWeapon;

    void Awake()
    {
        Initialize();
    }
    void Update()
    {
        if (!MenuHander.Instance.MenuIsOpen)
        {
            _equipmentWeapon.Action.WeaponAttack();
            if (!Input.GetButton("Attack"))
            {
                var swichFlg = Input.GetButton("SubWeaponSwitch");
                Debug.Log(PlayerAnimationState.Instance.IsAttack);
                SwichWeapon(swichFlg);
            }
        }
    }
    public void Initialize()
    {
        for(int index = 0;index < _weaponPrefabs.Length;index++)
        {
            _weaponPrefabs[index] = Instantiate(new WeaponPrefab((WeaponType)index), _weaponTransform[index]);
        }
        _mainWeaponBase = SetWeaponData.Instance.GetWeapon(WeaponType.SWORD);
        _subWeaponBase = SetWeaponData.Instance.GetWeapon(WeaponType.LANCE);

        SetEquipment(_mainWeaponBase, _subWeaponBase);
    }
    /// <summary>
    /// メイン武器・サブ武器の装備をボタンで切り替える関数
    /// </summary>
    void SwichWeapon(bool weaponSwitch)
    {
        WeaponDateEntity unEquipmentWeapon;

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
    /// <summary>メイン武器・サブ武器の装備をメニュー画面から切り替える関数・第一引数：メインかサブか指定・変更したい武器の名前</summary>
    /// <param name="equipmentType"></param>
    /// <param name="type"></param>
    public void ChangeWeapon(CommandType equipmentType, WeaponType type)
    {
        _equipmentWeapon.Base.RendererActive(false);
        switch (equipmentType)
        {
            case CommandType.MAIN:
                _mainWeaponBase = SetWeaponData.Instance.GetWeapon(type);
                break;
            case CommandType.SUB:
                _subWeaponBase = SetWeaponData.Instance.GetWeapon(type);
                break;
            default:
                break;
        }
        SetEquipment(_mainWeaponBase, _subWeaponBase);
        MainMenu.Instance.DisideElement(MainMenu.Instance.Element);
    }
    /// <summary>_equipmentWeaponの中身を変更する関数
    /// ・第一引数：装備させる武器
    /// ・第二引数：装備させていた武器</summary>
    /// <param name="equipmentWeapon"></param>
    /// <param name="unEquipmentWeapon"></param>
    void SetEquipment(WeaponDateEntity equipmentWeapon, WeaponDateEntity unEquipmentWeapon)
    {
        if (_equipmentWeapon.Type == WeaponType.NONE)
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

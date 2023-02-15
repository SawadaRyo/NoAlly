using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponPresenter : MonoBehaviour
{
    [SerializeField,Header("WeaponScriptableObjects")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField,Header("WeaponScriptableObjects")]
    PlayerContoller _playerContoller;

    [Space(15)]
    [Header("Model")]
    [SerializeField, Header("WeaponEquipment‚ðŠi”[‚·‚éŠÖ”")]
    WeaponEquipment _weaponEquipment = null;

    [Space(15)]
    [Header("View")]
    [SerializeField, Header("WeaponVisualController‚ðŠi”[‚·‚éŠÖ”")]
    WeaponVisualController _weaponVisual = null;
    [SerializeField, Header("WeaponProcessing‚ðŠi”[‚·‚éŠÖ”")]
    WeaponProcessing _weaponProcessing = null;

    SetWeaponData _weaponData = null;
    void Awake()
    {
        _weaponData = new SetWeaponData(_weaponScriptableObjects,_playerContoller);
        _weaponVisual.Initialize(_weaponData.GetAllWeapons);
        _weaponEquipment.Initialize(_weaponData);
        WeaponEquipmentState();
        WeaponProcessingState();
    }
    void WeaponEquipmentState()
    {
        //•Ší‚Ì‘•”õî•ñ
        _weaponEquipment.MainWeapon
            .Subscribe(mainWeapon =>
            {
                _weaponVisual.SetEquipment(mainWeapon, CommandType.MAIN);
                _weaponProcessing.TargetWeapon = _weaponEquipment
                                                 .CheckWeaponActive(_weaponVisual
                                                 .SwichWeapon(_weaponProcessing.IsSwichWeapon.Value));
            }).AddTo(this);
        _weaponEquipment.SubWeapon
            .Subscribe(subWeapon =>
            {
                _weaponVisual.SetEquipment(subWeapon, CommandType.SUB);
                _weaponProcessing.TargetWeapon = _weaponEquipment
                                                 .CheckWeaponActive(_weaponVisual
                                                 .SwichWeapon(_weaponProcessing.IsSwichWeapon.Value));
            }).AddTo(this);
    }
    void WeaponProcessingState()
    {
        _weaponProcessing.IsSwichWeapon
           .Subscribe(isSwich =>
           {
               if(!PlayerAnimationState.Instance.IsAttack)
               {
                   _weaponProcessing.TargetWeapon = _weaponEquipment.CheckWeaponActive(_weaponVisual.SwichWeapon(isSwich));
               }
           });
    }
}

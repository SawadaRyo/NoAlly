using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WeaponPresenter : MonoBehaviour
{
    [Header("Model")]
    [SerializeField, Tooltip("WeaponEquipment‚ðŠi”[‚·‚éŠÖ”")]
    WeaponEquipment _weaponEquipment = null;
    

    [Header("View")]
    [SerializeField, Tooltip("WeaponVisualController‚ðŠi”[‚·‚éŠÖ”")]
    WeaponVisualController _weaponVisual = null;
    [SerializeField, Tooltip("WeaponProcessing‚ðŠi”[‚·‚éŠÖ”")]
    WeaponProcessing _weaponProcessing = null;
    void Awake()
    {
        _weaponEquipment.Initialize();
        _weaponVisual.Initialize(_weaponEquipment.Data.GetAllWeapons,_weaponEquipment.MainWeapon.Value,_weaponEquipment.SubWeapon.Value);
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

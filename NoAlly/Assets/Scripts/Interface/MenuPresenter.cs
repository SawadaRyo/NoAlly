using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("WeaponEquipment‚ðŠi”[‚·‚éŠÖ”")] 
    WeaponEquipment _weaponEquipment = null;
    [SerializeField, Tooltip("MainMenu‚ðŠi”[‚·‚éŠÖ”")]
    MainMenu _mainMenu = null;
    void Start()
    {
        WeaponState();
    }
    void WeaponState()
    {
        //•Ší‚Ì‘•”õî•ñ
        _mainMenu.Main
            .Subscribe(mainWeapon =>
            {
                //_weaponEquipment.SetEquipment(CommandType.MAIN, mainWeapon.Type);
            }).AddTo(this);
        _mainMenu.Sub
            .Subscribe(subWeapon =>
            {
                //_weaponEquipment.SetEquipment(CommandType.SUB, subWeapon.Type);
            }).AddTo(this);
    }
}

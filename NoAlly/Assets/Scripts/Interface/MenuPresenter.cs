using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("WeaponEquipment���i�[����֐�")] 
    WeaponEquipment _weaponEquipment = null;
    [SerializeField, Tooltip("MainMenu���i�[����֐�")]
    MainMenu _mainMenu = null;
    void Start()
    {
        WeaponState();
    }
    void WeaponState()
    {
        //����̑������
        _mainMenu.Main
            .Subscribe(mainWeapon =>
            {
                _weaponEquipment.ChangeWeapon(CommandType.MAIN, mainWeapon.Type);
            }).AddTo(this);
        _mainMenu.Sub
            .Subscribe(subWeapon =>
            {
                _weaponEquipment.ChangeWeapon(CommandType.SUB, subWeapon.Type);
            }).AddTo(this);
    }
}

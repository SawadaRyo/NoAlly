using UnityEngine;
using UniRx;
using MVRP.Views;
using MVRP.Models;

public class Presenter : MonoBehaviour
{
    [SerializeField] WeaponEquipment _weaponEquipment = null;
    [SerializeField] MainMenu _mainMenu = null;
    [SerializeField] MenuHander _menuHander = null;

   

    void Start()
    {
        _menuHander.Init();
        _mainMenu.Init();
        _weaponEquipment.Init();
        WeaponState();
        //_mainMenu.Equipment(CommandType.MAIN, _mainMenu.WeaponsData.Value.WeaponData[0]);
        //_mainMenu.Equipment(CommandType.SUB, _mainMenu.WeaponsData.Value.WeaponData[1]);
    }
    void WeaponState()
    {
        _mainMenu.Main
            .Subscribe(mainWeapon =>
            {
                _weaponEquipment.ChangeWeapon(CommandType.MAIN, mainWeapon.Name);
            }).AddTo(this);
        _mainMenu.Sub
            .Subscribe(subWeapon =>
            {
                _weaponEquipment.ChangeWeapon(CommandType.SUB, subWeapon.Name);
            }).AddTo(this);
    }
}

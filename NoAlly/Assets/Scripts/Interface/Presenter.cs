using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Presenter : MonoBehaviour
{
    [SerializeField] WeaponEquipment _weaponEquipment = null;
    [SerializeField] MainMenu _mainMenu = null;
    [SerializeField] MenuHander _menuHander = null;

    [SerializeField] PlayerGauge _playerGauge = null;
    [SerializeField] GameObject[] _playerSlider = null;

    GaugeLarp _playerHPLarp = null;
    GaugeLarp _playerSAPLarp = null;
    Slider _hpSlider = null;
    Slider _sapSlider = null;

   

    void Start()
    {
        _playerHPLarp = _playerSlider[0].GetComponent<GaugeLarp>();
        _playerSAPLarp = _playerSlider[1].GetComponent<GaugeLarp>();
        _hpSlider = _playerSlider[0].GetComponent<Slider>();
        _sapSlider = _playerSlider[1].GetComponent<Slider>();

        _mainMenu.Init();
        _menuHander.Init();
        _weaponEquipment.Init();
        _playerGauge.Init();
        WeaponState();
        PlayerHpState();
    }
    void WeaponState()
    {
        //•Ší‚Ì‘•”õî•ñ
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
    void PlayerHpState()
    {
        _playerGauge.HP
            .Subscribe(hp =>
            {
                _playerHPLarp.SetSliderValue(_hpSlider, _playerGauge.HP.Value / _playerGauge.MaxHP);
            }).AddTo(this);
        _playerGauge.SAP
            .Subscribe(hp =>
            {
                _playerSAPLarp.SetSliderValue(_sapSlider, _playerGauge.SAP.Value / _playerGauge.MaxSAP);
            }).AddTo(this);
    }
}

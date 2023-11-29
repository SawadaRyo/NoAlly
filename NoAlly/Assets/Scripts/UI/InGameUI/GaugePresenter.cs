using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class GaugePresenter : MonoBehaviour
{
    [SerializeField] PlayerStatus _playerStatus = null;
    [SerializeField] GaugeLarp[] _playerSlider = null;

    GaugeLarp _playerHPLarp = null;
    GaugeLarp _playerSAPLarp = null;

   

    void Start()
    {
        _playerHPLarp = _playerSlider[0];
        _playerSAPLarp = _playerSlider[1];

        _playerStatus.Initialize();
        PlayerHpState();
    }

    void PlayerHpState()
    {
        _playerStatus.HP
            .Subscribe(hp =>
            {
                _playerHPLarp.SetSliderValue(_playerStatus.HP.Value / _playerStatus.MaxHP);
            }).AddTo(this);
        _playerStatus.SAP
            .Subscribe(hp =>
            {
                _playerSAPLarp.SetSliderValue(_playerStatus.SAP.Value / _playerStatus.MaxSAP);
            }).AddTo(this);
    }
}

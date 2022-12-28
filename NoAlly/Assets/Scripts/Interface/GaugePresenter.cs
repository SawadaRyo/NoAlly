using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class GaugePresenter : MonoBehaviour
{
    [SerializeField] PlayerStatus _playerStatus = null;
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

        PlayerHpState();
    }

    void PlayerHpState()
    {
        _playerStatus.HP
            .Subscribe(hp =>
            {
                _playerHPLarp.SetSliderValue(_hpSlider, _playerStatus.HP.Value / _playerStatus.MaxHP);
            }).AddTo(this);
        _playerStatus.SAP
            .Subscribe(hp =>
            {
                _playerSAPLarp.SetSliderValue(_sapSlider, _playerStatus.SAP.Value / _playerStatus.MaxSAP);
            }).AddTo(this);
    }
}

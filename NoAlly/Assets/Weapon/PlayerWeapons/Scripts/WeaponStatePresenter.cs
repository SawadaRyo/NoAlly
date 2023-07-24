//日本語コメント可
using UniRx;
using UnityEngine;

public class WeaponStatePresenter : MonoBehaviour
{
    [SerializeField]
    WeaponController _weaponController;
    [SerializeField]
    WeaponAnimator _weaponAnimator;
    [SerializeField]
    PlayerBehaviorController _inputToPlayer;
    [SerializeField]
    PlayerAnimator _playerAnimator;
    [SerializeField,Header("")]
    WeaponType[] mainAndSub = new WeaponType[2];

    void Start()
    {
        _weaponController.Initializer(mainAndSub[0], mainAndSub[1]);
        _weaponAnimator.Initializer();
        AnimationStateChacker();
        InputStateChacker();
    }

    void AnimationStateChacker()
    {
        _weaponController.EquipementWeapon
            .Skip(1)
            .Subscribe(weapon =>
            {
                _weaponAnimator.DeformationWeapon(weapon.WeaponData.TypeOfWeapon, _weaponController.CurrentElement.Value);
            });
        _weaponController.CurrentElement
            .Skip(1)
            .Subscribe(element =>
            {
                _weaponAnimator.DeformationWeapon(_weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon, element);
            });
    }
    void InputStateChacker()
    {
        _inputToPlayer.IsSwichWeapon
            .Subscribe(switchWeapon =>
            {
                _weaponController.SwichEquipmentWeapon(switchWeapon);
                
            });

        _inputToPlayer.InputAttackDown
            .Where(_ => _inputToPlayer.InputAttackDown.Value)
            .Subscribe(inputDown =>
            {
                _playerAnimator.GetPlayerAnimator.SetInteger("WeaponType", (int)_weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon);
                _playerAnimator.GetPlayerAnimator.SetTrigger("AttackTrigger");
            });
        _inputToPlayer.InputAttackCharge
            .Where(_ => _inputToPlayer.InputAttackCharge.Value)
            .Subscribe(inputCharge =>
            {
                float speedInCharge = _weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed(speedInCharge);
                _playerAnimator.GetPlayerAnimator.SetBool("Charge", _weaponController.EquipementWeapon.Value.Charge(true));
                
            });
        _inputToPlayer.InputAttackUp
            .Where(_ => _inputToPlayer.InputAttackUp.Value)
            .Subscribe(inputUp =>
            {
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed();
                _weaponController.EquipementWeapon.Value.Charge(false);
                _playerAnimator.GetPlayerAnimator.SetBool("Charge", _weaponController.EquipementWeapon.Value.Charge(false));
            });
    }
}

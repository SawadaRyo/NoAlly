//日本語コメント可
using UniRx;
using UnityEngine;

public class WeaponStatePresenter : MonoBehaviour
{
    [SerializeField,Header("武器本体の挙動")]
    WeaponController _weaponController;
    [SerializeField, Header("武器のアニメーションの挙動")]
    WeaponAnimator _weaponAnimator;
    [SerializeField, Header("プレイヤー本体の挙動")]
    PlayerBehaviorController _inputToPlayer;
    [SerializeField, Header("プレイヤーのアニメーションの挙動")]
    PlayerAnimatorController _playerAnimator;
    [SerializeField,Header("")]
    WeaponType[] mainAndSub = new WeaponType[2];

    void Start()
    {
        _weaponController.Initializer(mainAndSub[0], mainAndSub[1]);
        _weaponAnimator.Initializer();
        _playerAnimator.Initializer(_inputToPlayer);
        _playerAnimator.StateChacker();
        _playerAnimator.WeaponActionAnimation(_inputToPlayer,_weaponController);
        WeaponStateChacker();
        InputStateChacker();
        AttackStateChacker();
        InputUpdate();
    }

    void WeaponStateChacker()
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
    void AttackStateChacker()
    {
        _playerAnimator.IsAttack
            .Subscribe(isAttack =>
            {
                if(_weaponController.EquipementWeapon.Value is WeaponCombat combat)
                {
                    combat.AttackWeaponCombat(isAttack);
                }
            });
    }
    void InputStateChacker()
    {
        _inputToPlayer.IsSwichWeapon
            .Where(_ => _playerAnimator.IsAttack.Value == BoolAttack.NONE)
            .Subscribe(switchWeapon =>
            {
                _weaponController.SwichEquipmentWeapon(switchWeapon);
            });

       
        _inputToPlayer.InputAttackCharge
            .Where(_ => _inputToPlayer.InputAttackCharge.Value && _playerAnimator.AbleInput)
            .Subscribe(inputCharge =>
            {
                float speedInCharge = _weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed(speedInCharge);
                
            });
        _inputToPlayer.InputAttackUp
            .Where(_ => _inputToPlayer.InputAttackUp.Value && _playerAnimator.AbleInput)
            .Subscribe(inputUp =>
            {
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed();
                _weaponController.EquipementWeapon.Value.Charge(false);
            });
    }
    void InputUpdate()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _inputToPlayer.AbleMove = _playerAnimator.AbleMove;
                _inputToPlayer.AbleJump = _playerAnimator.AbleJump;
            }).AddTo(this);
    }
}

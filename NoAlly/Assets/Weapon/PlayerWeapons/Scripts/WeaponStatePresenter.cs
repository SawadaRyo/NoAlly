//日本語コメント可
using UniRx;
using UnityEngine;

public class WeaponStatePresenter : MonoBehaviour
{
    [SerializeField, Header("武器本体の挙動")]
    WeaponController _weaponController;
    [SerializeField, Header("武器のアニメーションの挙動")]
    WeaponAnimator _weaponAnimator;
    [SerializeField, Header("プレイヤー本体の挙動")]
    PlayerBehaviorController _inputToPlayer;
    [SerializeField, Header("プレイヤーのアニメーションの挙動")]
    PlayerAnimatorController _playerAnimator;
    [SerializeField, Header("")]
    WeaponType[] mainAndSub = new WeaponType[2];

    public void DoAttack()
    {
        _weaponController.EquipementWeapon.Value.AttackBehaviour();
    }

    void Start()
    {
        _weaponController.Initializer(mainAndSub[0], mainAndSub[1]);
        _weaponAnimator.Initializer();
        _playerAnimator.Initializer(_inputToPlayer);
        _playerAnimator.StateChacker();
        _playerAnimator.WeaponActionAnimation(_inputToPlayer, _weaponController);
        WeaponStateChacker();
        InputStateChacker();
        AttackStateChacker();
        InputUpdate();
    }
    void WeaponStateChacker()
    {
        _weaponController.EquipementWeapon
            .Subscribe(weapon =>
            {
                _weaponAnimator.DeformationWeapon(weapon.WeaponData.TypeOfWeapon, _weaponController.CurrentElement.Value);
            }).AddTo(this);
        _weaponController.CurrentElement
            .Subscribe(element =>
            {
                _weaponAnimator.DeformationWeapon(_weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon, element);
            }).AddTo(this);
    }
    void AttackStateChacker()
    {
        _playerAnimator.IsAttack
            .Subscribe(isParticle =>
            {
                if (_weaponController.EquipementWeapon.Value is WeaponCombat combat)
                {
                    combat.DoParticle(isParticle);
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
            }).AddTo(this);


        _inputToPlayer.InputAttackCharge
            .Where(_ => _inputToPlayer.InputAttackCharge.Value && _playerAnimator.AbleAttack)
            .Subscribe(inputCharge =>
            {
                float speedInCharge = _weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed(speedInCharge);
                _weaponController.EquipementWeapon.Value.Charging(inputCharge);
            }).AddTo(this);
        _inputToPlayer.InputAttackUp
            .Where(_ => _inputToPlayer.InputAttackUp.Value && _playerAnimator.AbleAttack)
            .Subscribe(inputUp =>
            {
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed();
                _weaponController.EquipementWeapon.Value.Charging(false);
            }).AddTo(this);
    }
    void InputUpdate()
    {
        _playerAnimator.AbleDash
            .Subscribe(ableDash =>
            {
                Debug.Log(ableDash);
                _inputToPlayer.AbleDash = ableDash;
            });
        _playerAnimator.AbleMove
            .Subscribe(ableMove =>
            {
                _inputToPlayer.AbleMove = ableMove;
            }).AddTo(this);
        _playerAnimator.AbleJump
            .Subscribe(ableJump =>
            {
                _inputToPlayer.AbleJump = ableJump;
            }).AddTo(this);
        _playerAnimator.AttackMoveSpeed
            .Subscribe(speed =>
            {
                if (speed != 0f)
                {
                    Vector2 vec = _inputToPlayer.MoveBehaviour.ActorMoveMethod(_inputToPlayer.CurrentVec.x, speed, _inputToPlayer.HitInfo);
                    _inputToPlayer.Rb.velocity = vec;
                }
            }).AddTo(this);
    }
}

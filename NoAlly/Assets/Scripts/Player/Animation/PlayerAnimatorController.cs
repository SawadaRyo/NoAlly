//日本語コメント可
using UnityEngine;
using UniRx;
using UniRx.Triggers;
//using UniRx.Triggers;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField]
    Animator _playerAnimator = null;

    [Tooltip("攻撃可能か判定する変数")]
    bool _ableAttack = true;
    [Tooltip("ダッシュ可能か判定する変数")]
    BoolReactiveProperty _ableDash = new(true);
    [Tooltip("移動可能か判定する変数")]
    BoolReactiveProperty _ableMove = new();
    [Tooltip("ジャンプ可能か判定する変数")]
    BoolReactiveProperty _ableJump = new();
    [Tooltip("攻撃時の移動速度")]
    FloatReactiveProperty _attackMovePlayerSpeed = new();
    [Tooltip("")]
    ReactiveProperty<BoolAttack> _isAttack = new();
    [Tooltip("アニメーションの状態")]
    ObservableStateMachineTrigger _trigger = null;

    float testTime = 0f;

    public bool AbleAttack => _ableAttack;
    public IReadOnlyReactiveProperty<bool> AbleDash => _ableDash;
    public IReadOnlyReactiveProperty<bool> AbleMove => _ableMove;
    public IReadOnlyReactiveProperty<bool> AbleJump => _ableJump;
    public IReadOnlyReactiveProperty<float> AttackMoveSpeed => _attackMovePlayerSpeed;
    public IReadOnlyReactiveProperty<BoolAttack> IsAttack => _isAttack;

    public void IsParticleFlg(BoolAttack boolAttack) => _isAttack.Value = boolAttack;
    public void SetMovePlayerCallback(float moveSpeed) => _attackMovePlayerSpeed.SetValueAndForceNotify(moveSpeed);

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="moveInput"></param>
    public void Initializer(PlayerBehaviorController moveInput)
    {
        _trigger = _playerAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        MoveAnimation(moveInput);
    }
    /// <summary>
    /// 攻撃時の移動関数設定
    /// </summary>
    /// <param name="movePlayer"></param>
    public void StateChacker()
    {
        _trigger
            .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
            .Subscribe(onStateInfo =>
            {
                if (onStateInfo.StateInfo.IsName("Idle"))
                {
                    _ableAttack = true;
                    _ableMove.Value = true;
                    _ableJump.Value = true;
                }
                else if (onStateInfo.StateInfo.IsName("Running Slide"))
                {
                    _ableDash.Value = false;
                }
                else if (onStateInfo.StateInfo.IsTag("AbleInput"))
                {
                    _ableMove.Value = true;
                    _ableJump.Value = true;
                }
                else if (onStateInfo.StateInfo.IsTag("DisableInput"))
                {
                    _ableAttack = false;
                    _ableMove.Value = false;
                    _ableJump.Value = false;
                }
                else if (onStateInfo.StateInfo.IsTag("DisableMove"))
                {
                    _ableMove.Value = false;
                    _ableJump.Value = false;
                }
                else if (onStateInfo.StateInfo.IsTag("DisableJump"))
                {
                    _ableJump.Value = false;
                }
            }).AddTo(this);
        _trigger
            .OnStateUpdateAsObservable()
            .Subscribe(onStateInfo =>
            {
                if (onStateInfo.StateInfo.IsName("Shoot"))
                {
                    testTime += Time.deltaTime;
                    _playerAnimator.SetFloat("MotionTime", testTime);
                }
            }).AddTo(this);
        _trigger
            .OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                if (onStateInfo.StateInfo.IsName("Running Slide"))
                {
                    _ableDash.Value = true;
                }
                else if (onStateInfo.StateInfo.IsName("Shoot"))
                {
                    testTime = 0f;
                    _playerAnimator.SetFloat("MotionTime", testTime);
                }
            }).AddTo(this);
    }

    /// <summary>
    /// プレイヤーの移動関係のアニメーションを管理する関数
    /// </summary>
    /// <param name="moveInput"></param>
    public void MoveAnimation(PlayerBehaviorController moveInput)
    {
        moveInput.IsDash
            .Where(_ => _ableDash.Value
                     && moveInput.CurrentMoveVector.Value.x != 0f
                     && moveInput.CurrentLocation.Value == StateOfPlayer.OnGround)
            .Subscribe(isDash =>
            {
                if (isDash) _playerAnimator.SetTrigger("Dudge");
            }).AddTo(moveInput);
        moveInput.CurrentLocation
            .Subscribe(currentLocation =>
            {
                _playerAnimator.SetBool("InAir", currentLocation == StateOfPlayer.InAir);
                _playerAnimator.SetBool("WallGrip", currentLocation == StateOfPlayer.GripingWall);
            }).AddTo(moveInput);
        moveInput.CurrentMoveVector
            .Subscribe(currentMoveVec =>
            {
                _playerAnimator.SetFloat("MoveSpeed", Mathf.Abs(moveInput.Rb.velocity.x));
            }).AddTo(moveInput);
        moveInput.WallBehaviour.Climbing
            .Subscribe(climbing =>
            {
                _playerAnimator.SetBool("Climbing", climbing);
            }).AddTo(moveInput);
    }
    /// <summary>
    /// プレイヤーの攻撃関係のアニメーションを管理する関数
    /// </summary>
    public void WeaponActionAnimation(IInputWeapon inputWeapon, IWeaponController weaponController)
    {
        inputWeapon.IsSwichWeapon
            .Where(_ => _isAttack.Value == BoolAttack.NONE)
            .Subscribe(isSwich =>
            {
                _playerAnimator.SetInteger("WeaponType", (int)weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon);
            }).AddTo(this);
        inputWeapon.InputAttackDown
            .Where(_ => inputWeapon.InputAttackDown.Value && _ableAttack)
            .Subscribe(inputDown =>
            {
                _playerAnimator.SetTrigger("AttackTrigger");
            }).AddTo(this);
        inputWeapon.InputAttackCharge
            .Subscribe(inputCharge =>
            {
                float speedInCharge = weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _playerAnimator.SetBool("Charge", weaponController.EquipementWeapon.Value.IsCharged);
                _playerAnimator.SetBool("Inputing", inputCharge);

            }).AddTo(this);
        inputWeapon.InputAttackUp
            .Where(_ => inputWeapon.InputAttackUp.Value)
            .Subscribe(inputUp =>
            {
                if (weaponController.EquipementWeapon.Value.IsCharged)
                {
                    _playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            }).AddTo(this);
    }
    void OnDisable()
    {
        _ableDash.Dispose();
        _ableMove.Dispose();
        _ableJump.Dispose();
        _isAttack.Dispose();
        _attackMovePlayerSpeed.Dispose();
    }
}

public struct CapsuleColliderValue
{
    public Vector3 center;
    public float height;

    public CapsuleColliderValue(Vector3 _center, float _higth)
    {
        center = _center;
        height = _higth;
    }
}


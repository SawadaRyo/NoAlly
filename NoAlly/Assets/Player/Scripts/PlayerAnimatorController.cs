//日本語コメント可
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
//using UniRx.Triggers;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField]
    Animator _playerAnimator = null;
    [SerializeField]
    CapsuleCollider _actorCollider = null;

    [Tooltip("攻撃可能か判定する変数")]
    bool _ableAttack = true;
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
    [Tooltip("")]
    CapsuleColliderValue _colliderValue;

    public bool AbleInput => _ableAttack;
    public IReadOnlyReactiveProperty<bool> AbleMove => _ableMove;
    public IReadOnlyReactiveProperty<bool> AbleJump => _ableJump;
    public IReadOnlyReactiveProperty<float> AttackMoveSpeed => _attackMovePlayerSpeed;
    public IReadOnlyReactiveProperty<BoolAttack> IsAttack => _isAttack;

    public void IsAttackFlg(BoolAttack boolAttack) => _isAttack.Value = boolAttack;
    public void SetMovePlayerCallback(float moveSpeed) => _attackMovePlayerSpeed.SetValueAndForceNotify(moveSpeed);

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="moveInput"></param>
    public void Initializer(PlayerBehaviorController moveInput)
    {
        _colliderValue = new CapsuleColliderValue(_actorCollider.center, _actorCollider.height);
        _trigger = _playerAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                var height = _playerAnimator.GetFloat("ColliderHeight");
                var center = new Vector3(_playerAnimator.GetFloat("ColliderCenterX")
                                       , _playerAnimator.GetFloat("ColliderCenterY")
                                       , _playerAnimator.GetFloat("ColliderCenterZ"));
                //Debug.Log(height);
                //Debug.Log(center);
                if (height != 0)
                {
                    _actorCollider.height = height;
                }
                else
                {
                    _actorCollider.height = _colliderValue.height;
                }

                if (center != Vector3.zero)
                {
                    _actorCollider.center = center;
                }
                else
                {
                    _actorCollider.center = _colliderValue.center;
                }

            }).AddTo(moveInput);
        MoveAnimation(moveInput);
    }
    /// <summary>
    /// 攻撃時の移動関数設定
    /// </summary>
    /// <param name="movePlayer"></param>
    public void StateChacker()
    {
        IDisposable weaponState = _trigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            if (onStateInfo.StateInfo.IsName("Idle"))
            {
                _ableAttack = true;
                _ableMove.Value = true;
                _ableJump.Value = true;
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
        });
    }
    /// <summary>
    /// プレイヤーの移動関係のアニメーションを管理する関数
    /// </summary>
    /// <param name="moveInput"></param>
    public void MoveAnimation(PlayerBehaviorController moveInput)
    {
        moveInput.IsDash
            .Where(_ => moveInput.AbleDash == true
                     && moveInput.CurrentMoveVector.Value.x != 0f
                     && moveInput.CurrentLocation.Value == StateOfPlayer.OnGround)
            .Subscribe(isDash =>
            {
                _playerAnimator.SetTrigger("Dudge");
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
            });
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
            });
        inputWeapon.InputAttackDown
            .Where(_ => inputWeapon.InputAttackDown.Value && _ableAttack)
            .Subscribe(inputDown =>
            {
                _playerAnimator.SetTrigger("AttackTrigger");
            });
        inputWeapon.InputAttackCharge
            .Subscribe(inputCharge =>
            {
                float speedInCharge = weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _playerAnimator.SetBool("Charge", weaponController.EquipementWeapon.Value.IsCharged);
                _playerAnimator.SetBool("Inputing", inputCharge);

            });
        inputWeapon.InputAttackUp
            .Where(_ => inputWeapon.InputAttackUp.Value)
            .Subscribe(inputUp =>
            {
                if (weaponController.EquipementWeapon.Value.IsCharged)
                {
                    _playerAnimator.SetTrigger("ChargeAttackTrigger");
                }
            });
    }

    void OnDisable()
    {
        _ableMove.Dispose();
        _ableJump.Dispose();
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


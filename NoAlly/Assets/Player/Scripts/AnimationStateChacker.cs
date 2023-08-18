using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using System;

public class AnimationStateChacker : MonoBehaviour
{
    [SerializeField]
    Animator _playerAnimator = null;
    [SerializeField]
    CapsuleCollider _actorCollider = null;

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
    [Tooltip("")]
    CapsuleColliderValue _colliderValue;

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
    /// 特定のステート検知と関数の実行を行う関数
    /// </summary>
    /// <param name="stateName">検知するステートの名前</param>
    /// <param name="type">検知するタイミング</param>
    /// <param name="action">呼び出す関数</param>
    public void StateChacker(string stateName, ObservableType type, Action action)
    {
        switch (type)
        {
            case ObservableType.SteteEnter:
                _trigger
                .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
                .Subscribe(onStateInfo =>
                {
                    if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                    {
                        action?.Invoke();
                    }
                });
                break;
            case ObservableType.SteteUpdate:
                _trigger
                .OnStateUpdateAsObservable()
                .Subscribe(onStateInfo =>
                {
                    if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                    {
                        action?.Invoke();
                    }
                });
                break;
            case ObservableType.SteteExit:
                _trigger
                .OnStateExitAsObservable()
                .Subscribe(onStateInfo =>
                {
                    if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                    {
                        action?.Invoke();
                    }
                });
                break;
        }
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
        _ableDash.Dispose();
        _ableMove.Dispose();
        _ableJump.Dispose();
        _isAttack.Dispose();
        _attackMovePlayerSpeed.Dispose();
    }
}

public enum ObservableType
{
    SteteEnter,
    SteteUpdate,
    SteteExit
}


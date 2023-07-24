//日本語コメント可
using UnityEngine;
using UniRx;
//using UniRx.Triggers;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _playerAnimator = null;
    [SerializeField]
    CapsuleCollider _actorCollider = null;

    //[Tooltip("Animationの遷移状況")]
    //ObservableStateMachineTrigger _trigger = default;
    [Tooltip("")]
    ReactiveProperty<BoolAttack> _isAttack = new();

    [Tooltip("")]
    CapsuleColliderValue _colliderValue;

    public Animator GetPlayerAnimator => _playerAnimator;
    public void IsAttackFlg(BoolAttack boolAttack) => _isAttack.Value = boolAttack;

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="moveInput"></param>
    public void Initialize(PlayerBehaviorController moveInput)
    {
        _colliderValue = new CapsuleColliderValue(_actorCollider.center, _actorCollider.height);
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
    public void WeaponActionAnimation(PlayerBehaviorController moveInput)
    {
        ////通常攻撃の処理
        if (Input.GetButtonDown("Attack"))
        {
            _playerAnimator.SetTrigger("AttackTrigger");
        }
        else
        {
            //溜め攻撃の処理(弓矢のアニメーションもこの処理）
            if (Input.GetButton("Attack"))
            {
                {
                    _playerAnimator.SetBool("Charge", true);
                }
            }
            else if (Input.GetButtonUp("Attack"))
            {
                _playerAnimator.SetBool("Charge", false);
            }
        }
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


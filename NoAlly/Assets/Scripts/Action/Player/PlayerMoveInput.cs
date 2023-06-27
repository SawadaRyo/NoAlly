//日本語コメント可
using System;
using UnityEngine;
using UniRx;

public class PlayerMoveInput : MonoBehaviour
{
    [SerializeField]
    PlayerParamater _playerParamater;
    [SerializeField]
    ActorStateJudge _stateJudge;
    [SerializeField]
    PlayerAnimator _playerAnimator;
    [SerializeField]
    Rigidbody _rb = null;

    bool _ableJump = true;
    float _h = 0f;
    float _v = 0f;
    Vector2 _currentMoveVector = Vector2.zero;
    RaycastHit _hitInfo;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    BoolReactiveProperty _onGroundPlayer = new();
    ReactiveProperty<StateOfPlayer> _playerLocation = new();

    delegate void PlayerInputUpdate();
    PlayerInputUpdate _playerInputUpdate;

    void Start()
    {
        _stateJudge.Initialize();
        _playerAnimator.Initialize(this);
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnEvent();
                OnUpdate();
            }).AddTo(this);
    }

    void OnEvent()
    {
        _onGroundPlayer
            .Subscribe(onGroundPlayer =>
            {
                if(onGroundPlayer)
                {
                    _playerLocation.Value = StateOfPlayer.OnGround;
                }
                else
                {
                    _playerLocation.Value = StateOfPlayer.InAri;
                }
            }).AddTo(this);
        _isDash
            .Skip(1)
            .Where(_isDash => true)
            .Subscribe(isDash =>
            {
                ActorMove.DodgeVec(_playerParamater.dashSpeed);
            }).AddTo(this);
        _isJump
            .Skip(1)
            .Where(isJump => true)
            .Subscribe(isJump =>
            {
                if (!_ableJump) return;
                ActorMove.ActorJumpMethod(_playerParamater.jumpPower
                                        , _rb
                                        , _currentMoveVector
                                        , _playerLocation.Value);
                _ableJump = false;
            }).AddTo(this);
    }

    void OnUpdate()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        _isJump.Value = Input.GetButtonDown("Jump");
        _currentMoveVector = _stateJudge.CurrentMoveVector(_h, _v);
        _onGroundPlayer.Value = _stateJudge.IsGrounded(_playerParamater.isGroundRengeRadios
                                                , _playerParamater.graundDistance
                                                , _playerParamater.groundMask
                                                , out _hitInfo);
        switch (_playerLocation.Value)
        {
            case StateOfPlayer.OnGround:
                _isDash.Value = Input.GetButtonDown("Dash");
                break;
            case StateOfPlayer.InAri:
                StateOfPlayer playerLocation = _stateJudge.IsHitWall(_playerParamater.walldistance
                                                    , _currentMoveVector
                                                    , _playerParamater.wallMask
                                                    , out RaycastHit[] hitInfo);
                ActorMove.BehaviourInWall(_playerParamater.wallSlideSpeed, _rb, hitInfo, playerLocation);
                break;
            default:
                break;
        }
        Vector3 moveVec = ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _hitInfo.normal);
        _rb.velocity = moveVec;
        ActorMove.RotateMethod(_playerParamater.turnSpeed, transform, _currentMoveVector);
    }
    void OnDisable()
    {
        _isDash.Dispose();
        _isJump.Dispose();
        _onGroundPlayer.Dispose();
        _playerLocation.Dispose();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_stateJudge.playerPartPos[2].position - new Vector3(0f, _playerParamater.graundDistance, 0f), _playerParamater.isGroundRengeRadios);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, (new Vector3(_h, 0f, 0f) + Vector3.up) * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, new Vector3(_currentMoveVector.x, _currentMoveVector.y, 0));

        Gizmos.DrawRay(_stateJudge.playerPartPos[0].position, transform.forward * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, transform.forward * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, transform.forward * _playerParamater.walldistance);
    }
}

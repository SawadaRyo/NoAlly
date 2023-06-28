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

    float _h = 0f;
    float _v = 0f;
    Vector2 _currentMoveVector = Vector2.zero;
    RaycastHit _hitInfo;
    BoolReactiveProperty _isJump = new();
    bool _ableJump = true;
    BoolReactiveProperty _isDash = new();
    BoolReactiveProperty _onGroundPlayer = new();
    ReactiveProperty<Vector2> _currentMove = new();
    ReactiveProperty<(bool,StateOfPlayer)> _currentLocation = new();

    delegate void PlayerInputUpdate();
    PlayerInputUpdate _playerInputUpdate;

    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<(bool, StateOfPlayer)> CurrentLocation => _currentLocation;
    public IReadOnlyReactiveProperty<Vector2> CurrentMove => _currentMove;

    void Start()
    {
        _stateJudge.Initialize();
        _playerAnimator.MoveAnimation(this);
        OnEvent();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnUpdate();
                _playerInputUpdate();
            }).AddTo(this);
    }

    void OnEvent()
    {
        _currentLocation
            .Subscribe(playerLocation =>
            {
                if(playerLocation.Item1)
                {
                    _playerInputUpdate = UpdateOnGround;
                    if (!_ableJump) _ableJump = true;
                }
                else
                {
                    switch (playerLocation.Item2)
                    {
                        case StateOfPlayer.GripingWall:
                            if (!_ableJump) _ableJump = true;
                            break;
                        case StateOfPlayer.InAri:
                            _playerInputUpdate = UpdateInAir;
                            break;
                        default:
                            break;
                    }
                }
            });
        _isDash
            .Skip(1)
            .Where(_ =>_isDash.Value == true)
            .Subscribe(isDash =>
            {
                ActorMove.DodgeVec(_rb,_currentMoveVector,_playerParamater.dashSpeed);
            }).AddTo(this);
        _isJump
            .Skip(1)
            .Where(_ => _isJump.Value == true)
            .Subscribe(isJump =>
            {
                if (!_ableJump) return;
                ActorMove.ActorJumpMethod(_playerParamater.jumpPower
                                        , _rb
                                        , _currentMoveVector
                                        , _currentLocation.Value);
                _ableJump = false;
            }).AddTo(this);
    }

    void OnUpdate()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        _isJump.Value = Input.GetButtonDown("Jump");

        _currentMoveVector = _stateJudge.CurrentMoveVector(_h, _v); //現在のプレイヤーの進行方向を代入
        _onGroundPlayer.Value = _stateJudge.IsGrounded(_playerParamater.isGroundRengeRadios //接地判定を代入
                                                , _playerParamater.graundDistance
                                                , _playerParamater.groundMask
                                                , out _hitInfo);

        StateOfPlayer playerLocation = _stateJudge.IsHitWall(_playerParamater.walldistance
                                                            , _currentMoveVector
                                                            , _playerParamater.wallMask
                                                            , out RaycastHit[] hitInfo);
        Vector3 moveVec = ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _hitInfo.normal);
        _rb.velocity = moveVec;
        _currentMove.SetValueAndForceNotify(_rb.velocity);
        ActorMove.RotateMethod(_playerParamater.turnSpeed, transform, _currentMoveVector);
    }

    void UpdateOnGround()
    {
        _isDash.Value = Input.GetButtonDown("Dash");
    }
    void UpdateInAir()
    {
        StateOfPlayer playerLocation = _stateJudge.IsHitWall(_playerParamater.walldistance
                                                            , _currentMoveVector
                                                            , _playerParamater.wallMask
                                                            , out RaycastHit[] hitInfo);
        ActorMove.BehaviourInWall(_playerParamater.wallSlideSpeed, _rb, hitInfo, playerLocation);
    }

    void OnDisable()
    {
        _isDash.Dispose();
        _isJump.Dispose();
        _onGroundPlayer.Dispose();
        _currentMove.Dispose();
        _currentLocation.Dispose();
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

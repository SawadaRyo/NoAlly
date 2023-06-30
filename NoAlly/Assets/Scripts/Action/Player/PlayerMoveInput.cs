//日本語コメント可
using System.Collections;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

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
    bool _ableDash = true;
    float _h = 0f;
    float _v = 0f;
    Vector2 _currentMoveVector = Vector2.zero;
    RaycastHit _groundHitInfo;
    RaycastHit[] _wallHitInfo;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    FloatReactiveProperty _currentMove = new();
    ReactiveProperty<(bool, StateOfPlayer)> _currentLocation = new();

    delegate void PlayerInputUpdate();
    PlayerInputUpdate _playerInputUpdate;

    public bool AbleDash => _ableDash;
    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<(bool, StateOfPlayer)> CurrentLocation => _currentLocation;
    public IReadOnlyReactiveProperty<float> CurrentMove => _currentMove;

    void Start()
    {
        _stateJudge.Initialize();
        _playerAnimator.MoveAnimation(this);
        OnEvent();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnUpdate();
            }).AddTo(this);
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                ActorMove.RotateMethod(_playerParamater.turnSpeed, transform, _currentMoveVector);
                _playerInputUpdate();
                _rb.velocity = new Vector3(_rb.velocity.x,ActorMove.ActorBehaviourInAir(_isJump.Value,_playerParamater.jumpPower, _currentLocation.Value).y,0f);
            }).AddTo(this);
    }

    void OnEvent()
    {
        _currentLocation
            .Subscribe(playerLocation =>
            {
                _ableDash = playerLocation.Item1;
                if (playerLocation.Item1)
                {
                    _playerInputUpdate = UpdateOnGround;
                    if (!_ableJump) _ableJump = true;
                }
                else
                {
                    _playerInputUpdate = UpdateInAir;
                    switch (playerLocation.Item2)
                    {
                        case StateOfPlayer.GripingWall:
                            if (!_ableJump) _ableJump = true;
                            break;
                        default:
                            break;
                    }
                }
            });
        _isJump
            .Skip(1)
            .Where(_ => _isJump.Value == true)
            .Subscribe(isJump =>
            {
                if (!_ableJump) return;
                //ActorMove.ActorJumpMethod(_playerParamater.jumpPower
                //                        , _rb
                //                        , _currentMoveVector
                //                        , _currentLocation.Value);
                _ableJump = false;
            }).AddTo(this);
    }

    void OnUpdate()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        _isDash.Value = Input.GetButtonDown("Dash");
        //_isJump.Value = Input.GetButtonDown("Jump");
        _isJump.Value = Input.GetButton("Jump");

        _currentMoveVector = _stateJudge.CurrentMoveVector(_h, _v); //現在のプレイヤーの進行方向を代入

        _currentLocation.Value =
            (
                 _stateJudge.IsGrounded(_playerParamater.isGroundRengeRadios //接地判定を代入
                                        , _playerParamater.graundDistance
                                        , _playerParamater.groundMask
                                        , out _groundHitInfo)
                ,
                 _stateJudge.IsHitWall(_playerParamater.walldistance //接壁判定を代入
                                    , _currentMoveVector
                                    , _playerParamater.wallMask
                                    , out _wallHitInfo)
            );

        _currentMove.SetValueAndForceNotify(_currentMoveVector.x * _playerParamater.speed);
    }
    void UpdateOnGround()
    {
        if (_ableDash && _currentMoveVector != Vector2.zero)
        {
            _rb.velocity = ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _groundHitInfo.normal);
        }
        else if(_currentMoveVector == Vector2.zero)
        {
            _rb.velocity = new(0f,_rb.velocity.y);
        }
    }
    IEnumerator UpdateDoDash()
    {
        _ableDash = false;
        float time = _playerParamater.dashInterval;
        while (time > 0)
        {
            _rb.velocity =
            ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _groundHitInfo.normal)
            + ActorMove.DodgeVec(_currentMoveVector, _playerParamater.dashSpeed);
            time -= Time.deltaTime;
            yield return null;
        }
        _ableDash = true;
    }
    void UpdateInAir()
    {
        ActorMove.ActorBehaviourInWall(_playerParamater.wallSlideSpeed, _rb, _wallHitInfo, _currentLocation.Value.Item2);
    }

    void OnDisable()
    {
        _isDash.Dispose();
        _isJump.Dispose();
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

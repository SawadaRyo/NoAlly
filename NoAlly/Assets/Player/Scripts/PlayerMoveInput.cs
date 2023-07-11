//日本語コメント可
using ActorBehaviour;
using System.Collections;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class PlayerMoveInput : MonoBehaviour
{
    [SerializeField]
    ActorParamater _playerParamater;
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
    StateMachine<PlayerMoveInput> _stateMachine = null;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    ReactiveProperty<Vector2> _currentMoveVector = new();
    ReactiveProperty<StateOfPlayer> _currentLocation = new();
    RaycastHit _hitInfo;

    delegate void PlayerInputUpdate();
    PlayerInputUpdate? _playerInputUpdate;

    public bool AbleDash => _ableDash;
    public StateMachine<PlayerMoveInput> PlayerStateMachine => _stateMachine;
    public ActorParamater PlayerParamater => _playerParamater;
    public Rigidbody Rb => _rb;
    public RaycastHit HitInfo => _hitInfo;
    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<bool> IsJump => _isJump;
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector => _currentMoveVector;
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation => _currentLocation;

    void Start()
    {
        _stateJudge.Initialize();
        _playerAnimator.MoveAnimation(this);
        SetState();
        OnEvent();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnUpdate();
            }).AddTo(this);
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                ActorMove.ActorRotateMethod(_playerParamater.turnSpeed, transform, _currentMoveVector.Value);
                _stateMachine.Update();
                //if (this._playerInputUpdate != null)
                //{
                //    _playerInputUpdate();
                //    //Debug.Log(_rb.velocity);
                //}
            }).AddTo(this);
    }
    void SetState()
    {
        _stateMachine = new StateMachine<PlayerMoveInput>(this);
        _stateMachine.AddAnyTransition<PlayerBehaviourOnGround>((int)StateOfPlayer.OnGround);
        _stateMachine.AddAnyTransition<PlayerBehaviorInAir>((int)StateOfPlayer.InAir);
        _stateMachine.AddAnyTransition<PlayerBehaviourOnWall>((int)StateOfPlayer.GripingWall);
        _stateMachine.AddAnyTransition<PlayerBehaviorDash>((int)StateOfPlayer.Dash);
        _stateMachine.Start<PlayerBehaviourOnGround>();
    }
    void OnEvent()
    {
        _currentLocation
            .Subscribe(playerLocation =>
            {
                switch (playerLocation)
                {
                    case StateOfPlayer.OnGround:
                        //_stateMachine.Dispatch((int)StateOfPlayer.OnGround);
                        //_playerInputUpdate = UpdateMove;
                        if (!_ableJump) _ableJump = true;
                        break;
                    case StateOfPlayer.InAir:
                        //_stateMachine.Dispatch((int)StateOfPlayer.InAir);
                        break;
                    case StateOfPlayer.GripingWall:
                        //_stateMachine.Dispatch((int)StateOfPlayer.GripingWall);
                        //_playerInputUpdate = UpdateOnWall;
                        if (!_ableJump) _ableJump = true;
                        break;
                    default:
                        break;
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

        _currentMoveVector.SetValueAndForceNotify(_stateJudge.CurrentMoveVector(_h, _v)); //現在のプレイヤーの進行方向を代入

        _currentLocation.Value = _stateJudge.ActorCurrentLocation(_ableJump, _playerParamater, _currentMoveVector.Value, out _hitInfo);
        Debug.Log(_currentLocation.Value);
    }

    void InputUpdateJump()
    {
        _isJump.Value = Input.GetButton("Jump");
        //if (IsJump.Value && _ableJumpInput)
        //{
        //    _actorFallJudge = ActorVec.Up;
        //    _ableJumpInput = false;
        //}
        //else if (_actorFallJudge == ActorVec.Down)
        //{
        //    _actorFallJudge = ActorVec.None;
        //}
        //else if (!IsJump.Value)
        //{
        //    //_timeInAir = 0f;
        //    _ableJumpInput = true;
        //}
    }

    void OnDisable()
    {
        _isDash.Dispose();
        _isJump.Dispose();
        _currentMoveVector.Dispose();
        _currentLocation.Dispose();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_stateJudge.playerPartPos[2].position - new Vector3(0f, _playerParamater.graundDistance, 0f), _playerParamater.isGroundRengeRadios);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, (new Vector3(_h, 0f, 0f) + Vector3.up) * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, new Vector3(_currentMoveVector.Value.x, _currentMoveVector.Value.y, 0));

        Gizmos.DrawRay(_stateJudge.playerPartPos[0].position, _currentMoveVector.Value * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, _currentMoveVector.Value * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, _rb.velocity * _playerParamater.walldistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, _hitInfo.normal * _playerParamater.walldistance);
    }
}

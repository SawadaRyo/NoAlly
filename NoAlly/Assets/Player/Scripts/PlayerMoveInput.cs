//日本語コメント可
using ActorBehaviour.Move;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using ActorBehaviour.Jump;
using ActorBehaviour.Wall;

public class PlayerMoveInput : MonoBehaviour,IHumanoid,IActor<PlayerMoveInput>
{
    [SerializeField]
    ActorParamater _playerParamater;
    [SerializeField]
    PlayerStateJudge _stateJudge;
    [SerializeField]
    PlayerAnimator _playerAnimator;
    [SerializeField]
    Rigidbody _rb = null;

    bool _ableJump = true;
    float _h = 0f;
    float _v = 0f;
    ActorAir _actorJump = null;
    ActorMove _actorMove = new ActorMove();
    ActorWall _actorWall = new ActorWall();

    StateMachine<PlayerMoveInput> _stateMachine = null;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    ReactiveProperty<Vector2> _currentMoveVector = new();
    ReactiveProperty<StateOfPlayer> _currentLocation = new();
    RaycastHit _hitInfo;

    public bool AbleDash
    {
        get 
        { 
            if(_stateMachine != null)
            {
                return !(_stateMachine.CurrentState is PlayerBehaviorDash);
            }
            return false;
        }
    }
    public StateMachine<PlayerMoveInput> PlayerStateMachine => _stateMachine;
    public ActorParamater PlayerParamater => _playerParamater;
    public Rigidbody Rb => _rb;
    public Vector3 GroundNormal => _stateJudge.GroundNormalChack(_currentMoveVector.Value,_playerParamater.graundDistance,_playerParamater.groundMask);
    public RaycastHit HitInfo => _hitInfo;
    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<bool> IsJump => _isJump;
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector => _currentMoveVector;
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation => _currentLocation;
    public ActorAir JumpBehaviour => _actorJump;
    public ActorMove MoveBehaviour => _actorMove;
    public ActorWall WallBehaviour => _actorWall;

    void Start()
    {
        _actorJump = new ActorAir(this);
        _stateJudge.Initialize();
        _playerAnimator.MoveAnimation(this);
        SetState();
        OnEvent();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnUpdateMove();
                OnUpdateJump();
            }).AddTo(this);
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                _actorMove.ActorRotateMethod(_playerParamater.turnSpeed, transform, _currentMoveVector.Value);
                _stateMachine.Update();
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
    }

    void OnUpdateMove()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        _isDash.Value = Input.GetButtonDown("Dash");

        _currentMoveVector.SetValueAndForceNotify(_stateJudge.CurrentMoveVectorNormal(_h, _v)); //現在のプレイヤーの進行方向を代入

        _currentLocation.Value = _stateJudge.ActorCurrentLocation(_ableJump, _playerParamater, _currentMoveVector.Value, out _hitInfo);
        Debug.Log(_currentLocation.Value);
    }

    void OnUpdateJump()
    {
        _isJump.SetValueAndForceNotify(Input.GetButton("Jump"));
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

//日本語コメント可
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using ActorBehaviour.Move;
using ActorBehaviour.Jump;
using ActorBehaviour.Wall;

public class PlayerBehaviorController : MonoBehaviour, IInput, IActor<PlayerBehaviorController>,IHumanoid
{
    [SerializeField,Header("プレイヤーの挙動に関するパラメーター")]
    ParamaterController _paramaterController = null;
    [SerializeField, Header("")]
    PlayerStateJudge _stateJudge;
    [SerializeField, Header("")]
    PlayerAnimator _playerAnimator;
    [SerializeField, Header("")]
    Rigidbody _rb = null;

    #region Paramater Move
    bool _ableJump = true;
    bool _ableMove = true;
    float _h = 0f;
    float _v = 0f;

    StateMachine<PlayerBehaviorController> _stateMachine = null;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    ReactiveProperty<Vector2> _currentMoveVector = new();
    ReactiveProperty<StateOfPlayer> _currentLocation = new();
    ActorAir _actorJump = null;
    ActorMove _actorMove = new ActorMove();
    ActorWall _actorWall = new ActorWall();
    RaycastHit _hitInfo;

    public bool AbleDash
    {
        get
        {
            if (_stateMachine != null)
            {
                return !(_stateMachine.CurrentState is PlayerBehaviorDash);
            }
            return false;
        }
    }
    public bool AbleMove { get => _ableMove; set => _ableMove = value; }
    public StateMachine<PlayerBehaviorController> PlayerStateMachine => _stateMachine;
    public ActorAir JumpBehaviour => _actorJump;
    public ActorMove MoveBehaviour => _actorMove;
    public ActorWall WallBehaviour => _actorWall;
    public Rigidbody Rb => _rb;
    public RaycastHit HitInfo => _hitInfo;
    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<bool> IsJump => _isJump;
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector => _currentMoveVector;
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation => _currentLocation;
    #endregion

    #region Paramater Attack
    [Tooltip("武器切り替えフラグ")]
    BoolReactiveProperty _isSwtchWeapon = new();
    [Tooltip("攻撃ボタン押し込み次判定")]
    BoolReactiveProperty _inputAttackDown = new();
    [Tooltip("攻撃ボタン長押し判定")]
    BoolReactiveProperty _inputAttackCharge = new();
    [Tooltip("攻撃ボタン離し判定")]
    BoolReactiveProperty _inputAttackUp = new();

    public ParamaterController ParamaterCon => _paramaterController;
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon => _isSwtchWeapon;
    public IReadOnlyReactiveProperty<bool> InputAttackDown => _inputAttackDown;
    public IReadOnlyReactiveProperty<bool> InputAttackCharge => _inputAttackCharge;
    public IReadOnlyReactiveProperty<bool> InputAttackUp => _inputAttackUp;

    public Vector3 GroundNormal => _stateJudge.GroundNormalChack(_currentMoveVector.Value
                                                               , _paramaterController.GetParamater.graundDistance
                                                               , _paramaterController.GetParamater.groundMask);
    #endregion

    void Start()
    {
        _paramaterController.Initializer();
        _actorJump = new ActorAir(this);
        _stateJudge.Initialize();
        _playerAnimator.Initialize(this);
        SetState();
        OnEvent();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                OnUpdateMove();
                OnUpdateAttack();
            }).AddTo(this);
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                _actorMove.ActorRotateMethod(_paramaterController.GetParamater.turnSpeed, transform, _currentMoveVector.Value);
                _stateMachine.Update();
            }).AddTo(this);
    }
    void SetState()
    {
        _stateMachine = new StateMachine<PlayerBehaviorController>(this);
        _stateMachine.AddAnyTransition<PlayerBehaviourOnGround>((int)StateOfPlayer.OnGround);
        _stateMachine.AddAnyTransition<PlayerBehaviorDash>((int)StateOfPlayer.Dash);
        _stateMachine.AddAnyTransition<PlayerBehaviorInAir>((int)StateOfPlayer.InAir);
        _stateMachine.AddAnyTransition<PlayerBehaviourOnWall>((int)StateOfPlayer.GripingWall);
        _stateMachine.AddAnyTransition<PlayerBehaviorClimbWall>((int)StateOfPlayer.GripingWallEdge);
        _stateMachine.AddAnyTransition<PlayerBehaviorClimbWall>((int)StateOfPlayer.HangingWallEgde);
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
                        if (!_ableJump) _ableJump = true;
                        break;
                    case StateOfPlayer.InAir:
                        break;
                    case StateOfPlayer.GripingWall:
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
        _isJump.SetValueAndForceNotify(Input.GetButton("Jump"));
        _isDash.Value = Input.GetButtonDown("Dash");

        _currentMoveVector.SetValueAndForceNotify(_stateJudge.CurrentMoveVectorNormal(_h, _v)); //現在のプレイヤーの進行方向を代入

        _currentLocation.Value = _stateJudge.ActorCurrentLocation(_ableJump, _paramaterController.GetParamater, _currentMoveVector.Value, out _hitInfo);
        Debug.Log(_currentLocation.Value);
    }
    void OnUpdateAttack()
    {
        if (!PlayerAttackStateController.Instance.AbleInput) return;
        //if (_inDeformation) return;

        if (!PlayerAttackStateController.Instance.IsAttack)
        {
            _isSwtchWeapon.Value = Input.GetButton("SubWeaponSwitch");
        }
        _inputAttackDown.Value = Input.GetButtonDown("Attack");
        _inputAttackCharge.SetValueAndForceNotify(Input.GetButton("Attack"));
        _inputAttackUp.Value = Input.GetButtonUp("Attack");
    }

    void OnDisable()
    {
        _isDash.Dispose();
        _isJump.Dispose();
        _currentMoveVector.Dispose();
        _currentLocation.Dispose();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_stateJudge.playerPartPos[2].position - new Vector3(0f, _paramaterController.GetParamater.graundDistance, 0f), _paramaterController.GetParamater.isGroundRengeRadios);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, (new Vector3(_h, 0f, 0f) + Vector3.up) * _paramaterController.GetParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, new Vector3(_currentMoveVector.Value.x, _currentMoveVector.Value.y, 0));

        Gizmos.DrawRay(_stateJudge.playerPartPos[0].position, _currentMoveVector.Value * _paramaterController.GetParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, _currentMoveVector.Value * _paramaterController.GetParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, _rb.velocity * _paramaterController.GetParamater.walldistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, _hitInfo.normal * _paramaterController.GetParamater.walldistance);
    }
#endif
}
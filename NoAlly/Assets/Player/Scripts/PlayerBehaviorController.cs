//日本語コメント可
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using ActorBehaviour.Move;
using ActorBehaviour.Jump;
using ActorBehaviour.Wall;

public class PlayerBehaviorController : MonoBehaviour, IInputPlayer, IActor<PlayerBehaviorController>, IHumanoid, IInputWeapon
{
    [SerializeField, Header("プレイヤーのパラメーターを管理")]
    ParamaterController _paramaterController = null;
    [SerializeField, Header("プレイヤーの接地、接壁判定")]
    PlayerStateJudge _stateJudge;
    [SerializeField, Header("プレイヤーのアニメーションを管理")]
    PlayerAnimatorController _playerAnimator;
    [SerializeField, Header("プレイヤーのRigidbody")]
    Rigidbody _rb = null;

    #region Paramater Move
    bool _ableJump = true;
    bool _ableWallJump = true;
    bool _ableMove = true;
    bool _inputDash = false;
    bool _ableDash = true;
    bool _ableInput = true;
    float _h = 0f;
    float _v = 0f;

    StateMachine<PlayerBehaviorController> _stateMachinePlayerMove = null;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    ReactiveProperty<Vector2> _currentMoveVector = new();
    ReactiveProperty<StateOfPlayer> _currentLocation = new();
    ActorAir _actorJump = null;
    ActorMove _actorMove = new ActorMove();
    ActorWall _actorWall = new ActorWall();
    Vector2 _currentMoveVec = Vector2.zero;
    RaycastHit _hitInfo;

    public bool InputDash => _inputDash;
    public bool AbleDash { get => _ableDash; set => _ableDash = _ableDash = value; }
    public bool AbleMove { get => _ableMove; set => _ableMove = value; }
    public bool AbleJump { get => _ableJump; set => _ableJump = value; }
    public bool AbleWallJump { get => _ableWallJump; set => _ableWallJump = value; }
    public bool AbleInput { get => _ableInput; set => _ableInput = value; }
    public Vector2 CurrentVec => _currentMoveVec;
    public StateMachine<PlayerBehaviorController> StateMachinePlayerMove => _stateMachinePlayerMove;
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
                                                               , _paramaterController.GetParamater.graundNormalDistance
                                                               , _paramaterController.GetParamater.groundMask);
    #endregion

    void Start()
    {
        _paramaterController.Initializer();
        _actorJump = new ActorAir(this);
        _stateJudge.Initialize();
        SetState();
        OnEvent();
        Observable.EveryUpdate()
            .Where(_ => _ableInput)
            .Subscribe(_ =>
            {
                OnUpdateMove();
                OnUpdateAttack();
            }).AddTo(this);
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                _stateMachinePlayerMove.Update();
            }).AddTo(this);
    }
    /// <summary>
    /// プレイヤーのステートを設定する関数
    /// </summary>
    void SetState()
    {
        _stateMachinePlayerMove = new StateMachine<PlayerBehaviorController>(this);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviourOnGround>((int)StateOfPlayer.OnGround);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviorDash>((int)StateOfPlayer.Dash);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviorInAir>((int)StateOfPlayer.InAir);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviourOnWall>((int)StateOfPlayer.GripingWall);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviorClimbWall>((int)StateOfPlayer.GripingWallEdge);
        _stateMachinePlayerMove.AddAnyTransition<PlayerBehaviorClimbWall>((int)StateOfPlayer.HangingWallEgde);
        _stateMachinePlayerMove.Start<PlayerBehaviourOnGround>();
    }
    /// <summary>
    /// 
    /// </summary>
    void OnEvent()
    {
        _currentLocation
            .Subscribe(playerLocation =>
            {
                switch (playerLocation)
                {
                    case StateOfPlayer.OnGround:
                        //if (!_ableJump) _ableJump = true;
                        break;
                    case StateOfPlayer.InAir:
                        break;
                    case StateOfPlayer.GripingWall:
                        //if (!_ableJump) _ableJump = true;
                        break;
                    default:
                        break;
                }
            }).AddTo(this);
    }
    void OnUpdateMove()
    {
        _h = Input.GetAxisRaw("Horizontal");
        //_v = Input.GetAxisRaw("Vertical");
        var ho = Input.GetAxisRaw("Mouse ScrollWheel");
        Debug.Log(ho);
        _isJump.SetValueAndForceNotify(Input.GetButton("Jump"));
        _isDash.Value = Input.GetButtonDown("Dash");
        _inputDash = Input.GetButton("Dash");

        if (_h != 0)
        {
            _currentMoveVec = _stateJudge.CurrentMoveVectorNormal(_h);
        }
        _currentMoveVector.SetValueAndForceNotify(_stateJudge.CurrentMoveVectorNormal(_h)); //現在のプレイヤーの進行方向を代入
        //Debug.Log(_currentMoveVector.Value);
        //Debug.Log(_currentLocation.Value);
        _currentLocation.Value = _stateJudge.ActorCurrentLocation(_paramaterController.GetParamater, _currentMoveVector.Value, out _hitInfo);
        //Debug.Log(_ableJump);
    }
    void OnUpdateAttack()
    {
        //if (_inDeformation) return;
        _isSwtchWeapon.SetValueAndForceNotify(Input.GetButton("SubWeaponSwitch"));
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
        //Gizmos.DrawWireSphere(_stateJudge.playerPartPos[2].position - new Vector3(0f, _paramaterController.GetParamater.graundDistance, 0f), _paramaterController.GetParamater.isGroundRengeRadios);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, new Vector3(0f, -_paramaterController.GetParamater.graundDistance, 0f));
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

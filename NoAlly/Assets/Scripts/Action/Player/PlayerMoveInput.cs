//日本語コメント可
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
    Vector2 _currentMoveVector = Vector2.zero;
    RaycastHit _hitInfo;
    BoolReactiveProperty _isJump = new();
    BoolReactiveProperty _isDash = new();
    FloatReactiveProperty _currentMove = new();
    ReactiveProperty<StateOfPlayer> _currentLocation = new();

    delegate void PlayerInputUpdate();
    PlayerInputUpdate? _playerInputUpdate;

    public bool AbleDash => _ableDash;
    public IReadOnlyReactiveProperty<bool> IsDash => _isDash;
    public IReadOnlyReactiveProperty<float> CurrentMove => _currentMove;
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation => _currentLocation;

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
                if(this._playerInputUpdate != null)
                {
                    _playerInputUpdate();
                }
            }).AddTo(this);
    }

    void OnEvent()
    {
        _currentLocation
            .Subscribe(playerLocation =>
            {
                switch (playerLocation)
                {
                    case StateOfPlayer.OnGround:
                        _playerInputUpdate = UpdateMove;
                        if (!_ableJump) _ableJump = true;
                        break;
                    case StateOfPlayer.InAri:
                        break;
                    case StateOfPlayer.GripingWall:
                        _playerInputUpdate = UpdateOnWall;
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
        _isJump.Value = Input.GetButton("Jump");

        _currentMoveVector = _stateJudge.CurrentMoveVector(_h, _v); //現在のプレイヤーの進行方向を代入
        _currentMove.SetValueAndForceNotify(_currentMoveVector.x * _playerParamater.speed);

        _currentLocation.Value =_stateJudge.ActorCurrentLocation(_playerParamater,_currentMoveVector,out _hitInfo);
        Debug.Log(_currentLocation.Value);
        
    }
    void UpdateMove()
    {
        if (_ableDash && _currentMoveVector != Vector2.zero)
        {
            _rb.velocity = ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _hitInfo.normal);
        }
        else if(_currentMoveVector == Vector2.zero)
        {
            _rb.velocity = new(0f,_rb.velocity.y);
        }

        _rb.velocity = new Vector3(_rb.velocity.x, ActorMove.ActorBehaviourInAir(_isJump.Value, _playerParamater.jumpPower,_hitInfo, _currentLocation.Value).y, 0f);
    }
    IEnumerator UpdateDoDash()
    {
        _ableDash = false;
        float time = _playerParamater.dashInterval;
        while (time > 0)
        {
            _rb.velocity =
            ActorMove.MoveMethod(_currentMoveVector.x, _playerParamater.speed, _rb, _hitInfo.normal)
            + ActorMove.DodgeVec(_currentMoveVector, _playerParamater.dashSpeed);
            time -= Time.deltaTime;
            yield return null;
        }
        _ableDash = true;
    }
    void UpdateOnWall()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, ActorMove.ActorBehaviourInAir(_isJump.Value, _playerParamater.jumpPower, _hitInfo, _currentLocation.Value).y, 0f);
        ActorMove.ActorBehaviourInWall(_playerParamater.wallSlideSpeed, _rb, _hitInfo, _currentLocation.Value);
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

        Gizmos.DrawRay(_stateJudge.playerPartPos[0].position, _currentMoveVector * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[1].position, _currentMoveVector * _playerParamater.walldistance);
        Gizmos.DrawRay(_stateJudge.playerPartPos[2].position, _currentMoveVector * _playerParamater.walldistance);
    }
}

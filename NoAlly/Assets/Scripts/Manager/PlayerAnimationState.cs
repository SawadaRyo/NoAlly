using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimationState : SingletonBehaviour<PlayerAnimationState>
{
    [Tooltip("移動可能か判定する変数")]
    bool _ableMove = true;
    [Tooltip("入力可能か判定する変数")]
    bool _ableInput = true;
    [Tooltip("「_able…」の変数の値を初期化するか判定する変数")]
    float _shootTiming = 0.35f;
    //.[Tooltip("近接攻撃の判定許可")]
    //bool _onHit = false;
    [Tooltip("攻撃アニメーションに遷移しているか確認する変数")]
    bool _isAttack = false;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    Animator _animator = default;
    [Tooltip("WeaponProcessingクラスを格納する変数")]
    WeaponProcessing _weaponProcessing;
    [Tooltip("PlayerControllerを格納する変数")]
    PlayerContoller _playerContoller = null;

    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;
    public bool IsAttack => _isAttack;

    private void Start()
    {
        //_onHit = false;
        _animator = GetComponent<Animator>();
        _playerContoller = GetComponent<PlayerContoller>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();  //AnimatorにアタッチしているObservableStateMachineTriggerを取ってくる
        _weaponProcessing = _playerContoller.GetComponent<WeaponProcessing>();

        DetectionStateEnterToNormalAction();
        DetectionStateEnterToAttack();
        DetectionStateUpdateToAttack();
        DetectionStateExitToAttack();


    }
    void DetectionStateEnterToNormalAction()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;　//現在のAnimatorの遷移状況

            //以下のコードに実行したい処理を書く
            if (info.IsTag("Idle"))
            {
                _ableMove = true;
                _isAttack = false;
                if (!_ableInput)
                {
                    _ableInput = true;
                }
            }
        }).AddTo(this);
    }
    void DetectionStateEnterToAttack()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo; //現在のAnimatorの遷移状況
            if (info.IsTag("GroundAttack"))
            {
                _ableMove = false;
                _isAttack = true;
            }
            else if (info.IsTag("AirAttack") || info.IsTag("MoveAttack"))
            {
                _ableInput = false;
                _isAttack = true;
            }
            else if (info.IsTag("GroundAttackFinish"))
            {
                if (info.IsName("SwordAttackFinish"))
                {

                }
                _ableInput = false;
                _ableMove = false;
                _isAttack = true;
            }

        }).AddTo(this);
    }
    void DetectionStateUpdateToAttack()
    {
        IDisposable bowAnimationUpdate = _trigger
        .OnStateUpdateAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsTag("GroundAttackFinish"))
            {
                if (info.IsName("SwordAttackFinish"))
                {

                }
            }
        }).AddTo(this);
    }
    void DetectionStateExitToAttack()
    {
        IDisposable exitState = _trigger
        .OnStateExitAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsTag("GroundAttack") || info.IsName("JumpEnd"))
            {
                Debug.Log("Exit");
            }
            else if (info.IsTag("AirAttack") || info.IsTag("AirAttack"))
            {
                _ableInput = true;
            }
        }).AddTo(this);
    }
    void PlayerAnimationEnter()
    {
        IDisposable bowAnimationUpdate = _trigger
        .OnStateUpdateAsObservable()
        .Where(x => x.StateInfo.IsTag("Shoot"))
        .Where(x => x.StateInfo.normalizedTime >= _shootTiming)
        .Take(1)
        .Subscribe(x =>
        {
            //_eWeapon.WeaponChargeAttackMethod(_eWeapon.ChrageCount);
        }).AddTo(this);
    }

    //AnimationEventで呼ぶ関数
    void AttackToCombatWeapon(BoolAttack isAttack)
    {
        _weaponProcessing.TargetWeapon.Action.LoopJud(isAttack);
        //_weaponProcessing.TargetWeapon.Base.LoopJud(_onHit);
    }
    void FinishAttackMove(int moveSpeed)
    {
        Vector3 vec = Vector3.zero;
        switch (_playerContoller.Vec)
        {
            case PlayerContoller.PlayerVec.RIGHT:
                vec = Vector3.right;
                break;
            case PlayerContoller.PlayerVec.LEFT:
                vec = Vector3.left;
                break;
            default:
                break;
        }
        Vector3 onPlane = Vector3.ProjectOnPlane(vec, _playerContoller.HitInfo.normal);
        _playerContoller.Rb.velocity = onPlane * moveSpeed;
    }
    void BulletFire()
    {
        _weaponProcessing.TargetWeapon.Action.WeaponChargeAttackMethod();
        _weaponProcessing.TargetWeapon.Action.ResetValue();
    }
}

public enum BoolAttack
{
    NONE,
    ATTACKING
}

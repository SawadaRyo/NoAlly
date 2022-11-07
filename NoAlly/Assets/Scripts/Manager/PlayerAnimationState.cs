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
    bool _reset;
    float _shootTiming = 0.35f;
    [Tooltip("近接攻撃の判定許可")]
    bool _onHit = false;
    [Tooltip("攻撃アニメーションに遷移しているか確認する変数")]
    bool _isAttack = false;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    Animator _animator = default;
    [Tooltip("WeaponEquipmentクラスを格納する変数")]
    WeaponEquipment _weaponEquipment;
    [Tooltip("PlayerControllerを格納する変数")]
    PlayerContoller _playerContoller = null;

    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;
    public bool IsAttack => _isAttack;

    private void Start()
    {
        _onHit = false;
        _animator = GetComponent<Animator>();
        _playerContoller = GetComponent<PlayerContoller>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();  //AnimatorにアタッチしているObservableStateMachineTriggerを取ってくる
        _weaponEquipment = WeaponEquipment.Instance;
        DetectionStateEnterToAttack();
        DetectionStateUpdateToAttack();
        DetectionStateExitToAttack();
    }

    void DetectionStateEnterToAttack()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;　//現在のAnimatorの遷移状況

            //以下のコードに実行したい処理を書く
            if(info.IsTag("Idle"))
            {
                if (_reset)
                {
                    _ableMove = true;
                    _isAttack = false;
                    if (!_ableInput)
                    {
                        _ableInput = true;
                    }
                    _reset = false;
                }
            }
            else if (info.IsTag("GroundAttack"))
            {
                _ableMove = false;
            }
            else if (info.IsTag("AirAttack") || info.IsTag("MoveAttack"))
            {
                _ableInput = false;
            }
            else if (info.IsTag("GroundAttackFinish"))
            {
                if (info.IsName("SwordAttackFinish"))
                {
                    
                }
                _ableInput = false;
                _ableMove = false;
            }
            _isAttack = true;

            if (!_reset) _reset = true;

        }).AddTo(this);

        

        IDisposable updateState = _trigger
        .OnStateUpdateAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsTag("Ground"))
            {
                //if(info.length == )
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
    //void ResetInput()
    //{
    //    if (_reset)
    //    {
    //        _ableMove = true;
    //        _isAttack = false;
    //        if (!_ableInput)
    //        {
    //            _ableInput = true;
    //        }
    //        _reset = false;
    //    }
    //}
    void AttackToCombatWeapon()
    {
        _onHit = !_onHit;
        StartCoroutine(_weaponEquipment.EquipeWeapon.Base.LoopJud(_onHit));
        //_weaponEquipment.EquipeCombatWeapon.LoopJud(_onHit);
    }
    void BulletFIre()
    {
        float chrageCount = WeaponEquipment.Instance.EquipeWeapon.ChrageCount;
        WeaponEquipment
            .Instance.EquipeWeapon
            .WeaponChargeAttackMethod(chrageCount);
    }
}

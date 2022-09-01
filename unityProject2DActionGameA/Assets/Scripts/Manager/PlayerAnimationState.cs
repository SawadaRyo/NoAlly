using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimationState : SingletonBehaviour<PlayerAnimationState>
{
    [Tooltip("ˆÚ“®‰Â”\‚©”»’è‚·‚é•Ï”")]
    bool _ableMove = true;
    [Tooltip("“ü—Í‰Â”\‚©”»’è‚·‚é•Ï”")]
    bool _ableInput = true;
    [Tooltip("Animation‚Ì‘JˆÚó‹µ")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("Player‚ÌAnimator‚ðŠi”[‚·‚é•Ï”")]
    Animator _animator = default;
    [Tooltip("WeaponEquipment‚ðŠi”[‚·‚é•Ï”")]
    WeaponEquipment _weaponEquipment;

    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
        _weaponEquipment = WeaponEquipment.Instance;
        DetectionState();
    }

    void DetectionState()
    {
        //m_trigger
        //.OnStateEnterAsObservable()
        //.Subscribe(onStateInfo =>
        //{
        //    if (onStateInfo.StateInfo.IsTag("Ground") || onStateInfo.StateInfo.IsName("JumpEnd"))
        //    {
        //        Debug.Log("Enter");
        //        m_ableMove = false;
        //        if (onStateInfo.StateInfo.IsName($"{m_weaponEquipment.name}+AttackFinish"))
        //        {
        //            m_ableInput = false;
        //        }
        //    }
        //}).AddTo(this); 
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsTag("GroundAttack"))
            {
                _ableMove = false;
            }
            else if (info.IsTag("GroundAttackFinish"))
            {
                _ableInput = false;
                _ableMove = false;
            }
            else if(info.IsTag("Idle"))
            {
                _ableMove = true;
                if (!_ableInput)
                {
                    _ableInput = true;
                }
            }

        }).AddTo(this);

        IDisposable exitState =_trigger
            .OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (info.IsTag("GroundAttack") || info.IsName("JumpEnd"))
                {
                    Debug.Log("Exit");
                    
                }
            }).AddTo(this);

        IDisposable updateState = _trigger
        .OnStateUpdateAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsTag("Ground") || info.IsName("NoGround.JumpEnd"))
            {
                //if(info.length == )
            }
        }).AddTo(this);
    }
}

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

    float _shootTiming = 0.35f;
    [Tooltip("Animation‚Ì‘JˆÚó‹µ")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("Player‚ÌAnimator‚ðŠi”[‚·‚é•Ï”")]
    Animator _animator = default;
    [Tooltip("WeaponEquipmentƒNƒ‰ƒX‚ðŠi”[‚·‚é•Ï”")]
    WeaponEquipment _weaponEquipment;
    [Tooltip("‘•”õ’†‚Ì•Ší‚ÌWeaponAction‚ðŠi”[‚·‚é•Ï”")]
    WeaponAction _eWeapon = default;

    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
        _weaponEquipment = WeaponEquipment.Instance;
        _eWeapon = _weaponEquipment.EquipeWeaponAction;
        DetectionState();
        AnimationEnter();
    }

    void DetectionState()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            //Debug.Log(onStateInfo);
            if (info.IsTag("Idle"))
            {
                _ableMove = true;
                if (!_ableInput)
                {
                    _ableInput = true;
                }
            }
            else if (info.IsTag("GroundAttack"))
            {
                _ableMove = false;
            }
            else if (info.IsTag("GroundAttackFinish"))
            {
                _ableInput = false;
                _ableMove = false;
            }

        }).AddTo(this);

        IDisposable exitState = _trigger
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

    void AnimationEnter()
    {
        IDisposable bowAnimationEnter = _trigger
        .OnStateEnterAsObservable()
        .Where(x => x.StateInfo.IsTag("Shoot"))
        .Where(x => x.StateInfo.normalizedTime >= _shootTiming)
        .Take(1)
        .Subscribe(x =>
        {
            _eWeapon.WeaponChargeAttackMethod(_eWeapon.ChrageCount);
        }).AddTo(this);
    }
}

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

    float _shootTiming = 0.35f;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    Animator _animator = default;
    [Tooltip("WeaponEquipmentクラスを格納する変数")]
    WeaponEquipment _weaponEquipment;
    [Tooltip("装備中の武器のWeaponActionを格納する変数")]
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

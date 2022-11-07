using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimationState : SingletonBehaviour<PlayerAnimationState>
{
    [Tooltip("�ړ��\�����肷��ϐ�")]
    bool _ableMove = true;
    [Tooltip("���͉\�����肷��ϐ�")]
    bool _ableInput = true;
    [Tooltip("�u_able�c�v�̕ϐ��̒l�����������邩���肷��ϐ�")]
    bool _reset;
    float _shootTiming = 0.35f;
    [Tooltip("�ߐڍU���̔��苖��")]
    bool _onHit = false;
    [Tooltip("�U���A�j���[�V�����ɑJ�ڂ��Ă��邩�m�F����ϐ�")]
    bool _isAttack = false;
    [Tooltip("Animation�̑J�ڏ�")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("Player��Animator���i�[����ϐ�")]
    Animator _animator = default;
    [Tooltip("WeaponEquipment�N���X���i�[����ϐ�")]
    WeaponEquipment _weaponEquipment;
    [Tooltip("PlayerController���i�[����ϐ�")]
    PlayerContoller _playerContoller = null;

    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;
    public bool IsAttack => _isAttack;

    private void Start()
    {
        _onHit = false;
        _animator = GetComponent<Animator>();
        _playerContoller = GetComponent<PlayerContoller>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();  //Animator�ɃA�^�b�`���Ă���ObservableStateMachineTrigger������Ă���
        _weaponEquipment = WeaponEquipment.Instance;
        DetectionStateEnterToAttack();
        DetectionStateUpdateToAttack();
        DetectionStateExitToAttack();
    }

    void DetectionStateEnterToAttack()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()�@�@//Animation�̑J�ڊJ�n�����m
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;�@//���݂�Animator�̑J�ڏ�

            //�ȉ��̃R�[�h�Ɏ��s����������������
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

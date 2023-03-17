using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimationState : SingletonBehaviour<PlayerAnimationState>
{
    [SerializeField,Tooltip("WeaponProcessing�N���X���i�[����ϐ�")]
    WeaponProcessing _weaponProcessing;

    [Tooltip("�ړ��\�����肷��ϐ�")]
    bool _ableMove = true;
    [Tooltip("���͉\�����肷��ϐ�")]
    bool _ableInput = true;
    [Tooltip("�u_able�c�v�̕ϐ��̒l�����������邩���肷��ϐ�")]
    float _shootTiming = 0.35f;
    [Tooltip("�U���A�j���[�V�����ɑJ�ڂ��Ă��邩�m�F����ϐ�")]
    bool _isAttack = false;
    ReactiveProperty<BoolAttack> _boolAttack = new ReactiveProperty<BoolAttack>();
    [Tooltip("Animation�̑J�ڏ�")]
    ObservableStateMachineTrigger _trigger = default;
    [Tooltip("Player��Animator���i�[����ϐ�")]
    Animator _animator = default;
    [Tooltip("PlayerController���i�[����ϐ�")]
    PlayerContoller _playerContoller = null;


    public bool AbleMove => _ableMove;
    public bool AbleInput => _ableInput;
    public bool IsAttack => _isAttack;
    public IReadOnlyReactiveProperty<BoolAttack> Hit => _boolAttack;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerContoller = GetComponent<PlayerContoller>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();  //Animator�ɃA�^�b�`���Ă���ObservableStateMachineTrigger������Ă���

        DetectionStateEnterToNormalAction();
        DetectionStateEnterToAttack();
        DetectionStateUpdateToAttack();
        DetectionStateExitToAttack();
    }
    void DetectionStateEnterToNormalAction()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()�@�@//Animation�̑J�ڊJ�n�����m
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;�@//���݂�Animator�̑J�ڏ�

            //�ȉ��̃R�[�h�Ɏ��s����������������
            if (info.IsTag("Idle"))
            {
                _ableMove = true;
                _isAttack = false;
                if (!_ableInput)
                {
                    _ableInput = true;
                }
            }
            else if(info.IsTag("DisableMove"))
            {
                _ableMove = false;
            }
        }).AddTo(this);
    }
    void DetectionStateEnterToAttack()
    {
        IDisposable enterState = _trigger
        .OnStateEnterAsObservable()�@�@//Animation�̑J�ڊJ�n�����m
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo; //���݂�Animator�̑J�ڏ�
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
            if (info.IsTag("DisableMove"))
            {
                _ableMove = true;
            }
            else if (info.IsTag("AirAttack"))
            {
                _ableInput = true;
            }
        }).AddTo(this);
    }


    //AnimationEvent�ŌĂԊ֐�
    void AttackToCombatWeapon(BoolAttack isAttack) => _boolAttack.Value = isAttack;
    void AttackToArrowWeapon()
    {
        if(_weaponProcessing.TargetWeapon.Base is IArrowWeapon arrow)
        {
            arrow.InsBullet(_weaponProcessing.TargetWeapon.Action);
        }
    }
    void FinishAttackMove(int moveSpeed)
    {
        Vector3 vec = Vector3.zero;
        switch (_playerContoller.Vec)
        {
            case PlayerVec.RIGHT:
                vec = Vector3.right;
                break;
            case PlayerVec.LEFT:
                vec = Vector3.left;
                break;
            default:
                break;
        }
        Vector3 onPlane = Vector3.ProjectOnPlane(vec, _playerContoller.HitInfo.normal);
        _playerContoller.Rb.velocity = onPlane * moveSpeed;
    }
    void MoveSound(int soundNum)
    {
        GameManager.InstanceSM.CallSound(SoundUsage.PLAYERMOVE, SoundType.SE, soundNum);
    }
    void InsBullet(Transform muzzlePos)
    {
        if(_weaponProcessing.TargetWeapon.Type == WeaponType.ARROW)
        {
            //_weaponProcessing.TargetWeapon.Action.WeaponChargeAttackMethod()
        }
    }

    private void OnDisable()
    {
        _boolAttack.Dispose();
    }
}

public enum BoolAttack
{
    NONE,
    ATTACKING
}

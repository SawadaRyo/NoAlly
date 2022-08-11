using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimationState :SingletonBehaviour<PlayerAnimationState>
{
    [Tooltip("移動可能か判定する変数")]
    bool m_ableMove = true;
    [Tooltip("")]
    bool m_ableInput = true;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger m_trigger = default;
    [Tooltip("PlayerのAnimatorを格納する変数")]
    Animator m_animator = default;
    [Tooltip("WeaponEquipmentを格納する変数")]
    WeaponEquipment m_weaponEquipment;

    public bool AbleMove => m_ableMove;
    public bool AbleInput => m_ableInput;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_trigger = m_animator.GetBehaviour<ObservableStateMachineTrigger>();
        m_weaponEquipment = WeaponEquipment.Instance;
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
            m_trigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("NoGround.JumpEnd"))
            .Subscribe(x =>
            {
                    Debug.Log("Enter");
                    m_ableMove = false;
            }).AddTo(this);

            m_trigger
            .OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (info.IsTag("Base Layer.Ground") || info.IsName("JumpEnd"))
                {
                    Debug.Log("Exit");
                    m_ableMove = true;
                    if(!m_ableInput)
                    {
                        m_ableInput = true;
                    }
                }
            }).AddTo(this);

            m_trigger
            .OnStateUpdateAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (info.IsTag("Base Layer.Ground") || info.IsName("JumpEnd"))
                {
                    m_ableMove = true;
                    if (!m_ableInput)
                    {
                        m_ableInput = true;
                    }
                }
            }).AddTo(this);
    }
}

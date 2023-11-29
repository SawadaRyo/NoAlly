using UniRx.Triggers;
using UniRx;
using UnityEngine;
using System;


namespace StateChacker
{
    public class AnimationStateChacker:MonoBehaviour
    {
        static AnimationStateChacker _instance = null;
        /// <summary>
        /// �������֐�
        /// </summary>
        /// <param name="moveInput"></param>
        public static AnimationStateChacker Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("�C���X�^���X����������Ă��܂���");
                }
                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance != null) return;
            _instance = this;
        }
        public void OnDestroy()
        {
            _instance = null;
        }

        /// <summary>
        /// ����̃X�e�[�g���m�Ɗ֐��̎��s���s���֐�
        /// </summary>
        /// <param name="stateName">���m����X�e�[�g�̖��O</param>
        /// <param name="type">���m����^�C�~���O</param>
        /// <param name="action">�Ăяo���֐�</param>
        public void StateChacker(Animator animator, string stateName, ObservableType type, Action action)
        {
            ObservableStateMachineTrigger trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
            switch (type)
            {
                case ObservableType.SteteEnter:
                    trigger
                    .OnStateEnterAsObservable()  //Animation�̑J�ڊJ�n�����m
                    .Subscribe(onStateInfo =>
                    {
                        if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                        {
                            action?.Invoke();
                        }
                    }).AddTo(animator);
                    break;
                case ObservableType.SteteUpdate:
                    trigger
                    .OnStateUpdateAsObservable()
                    .Subscribe(onStateInfo =>
                    {
                        if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                        {
                            action?.Invoke();
                        }
                    }).AddTo(animator);
                    break;
                case ObservableType.SteteExit:
                    trigger
                    .OnStateExitAsObservable()
                    .Subscribe(onStateInfo =>
                    {
                        if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                        {
                            action?.Invoke();
                        }
                    }).AddTo(animator);
                    break;
            }
        }
    }
}



public enum ObservableType
{
    SteteEnter,
    SteteUpdate,
    SteteExit
}
public enum BoolAttack
{
    NONE,
    ATTACKING
}


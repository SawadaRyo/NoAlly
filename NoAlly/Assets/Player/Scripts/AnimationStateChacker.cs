using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using System;

public class AnimationStateChacker
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
            if(_instance == null)
            {
                Debug.LogError("�C���X�^���X����������Ă��܂���");
            }
            return _instance;
        }
    }

    /// <summary>
    /// ����̃X�e�[�g���m�Ɗ֐��̎��s���s���֐�
    /// </summary>
    /// <param name="stateName">���m����X�e�[�g�̖��O</param>
    /// <param name="type">���m����^�C�~���O</param>
    /// <param name="action">�Ăяo���֐�</param>
    public void StateChacker(ObservableStateMachineTrigger trigger, string stateName, ObservableType type, Action action)
    {
        switch (type)
        {
            case ObservableType.SteteEnter:
                trigger
                .OnStateEnterAsObservable()�@�@//Animation�̑J�ڊJ�n�����m
                .Subscribe(onStateInfo =>
                {
                    if (onStateInfo.StateInfo.IsName(stateName) || onStateInfo.StateInfo.IsTag(stateName))
                    {
                        action?.Invoke();
                    }
                });
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
                });
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
                });
                break;
        }
    }
}

public enum ObservableType
{
    SteteEnter,
    SteteUpdate,
    SteteExit
}


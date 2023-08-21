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
    /// 初期化関数
    /// </summary>
    /// <param name="moveInput"></param>
    public static AnimationStateChacker Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("インスタンスが生成されていません");
            }
            return _instance;
        }
    }

    /// <summary>
    /// 特定のステート検知と関数の実行を行う関数
    /// </summary>
    /// <param name="stateName">検知するステートの名前</param>
    /// <param name="type">検知するタイミング</param>
    /// <param name="action">呼び出す関数</param>
    public void StateChacker(ObservableStateMachineTrigger trigger, string stateName, ObservableType type, Action action)
    {
        switch (type)
        {
            case ObservableType.SteteEnter:
                trigger
                .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
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


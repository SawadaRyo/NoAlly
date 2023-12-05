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
        /// 初期化関数
        /// </summary>
        /// <param name="moveInput"></param>
        public static AnimationStateChacker Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("インスタンスが生成されていません");
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
        /// 特定のステート検知と関数の実行を行う関数
        /// </summary>
        /// <param name="stateName">検知するステートの名前</param>
        /// <param name="type">検知するタイミング</param>
        /// <param name="action">呼び出す関数</param>
        public void StateChacker(Animator animator, string stateName, ObservableType type, Action action)
        {
            ObservableStateMachineTrigger trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
            switch (type)
            {
                case ObservableType.SteteEnter:
                    trigger
                    .OnStateEnterAsObservable()  //Animationの遷移開始を検知
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


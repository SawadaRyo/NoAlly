//日本語コメント可
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField,Header("")]
    ObjectBase _targetWeapon = null;

    [Tooltip("武器が変形中かどうか")]
    bool _inBoostMode = false;
    [Tooltip("武器のアニメーションの状態")]
    ObservableStateMachineTrigger _weaponAnimationTrigger = null;

    public bool InBoostMode => _inBoostMode;

    public void Initializer()
    {
        _weaponAnimationTrigger = _targetWeapon.ObjectAnimator.GetBehaviour<ObservableStateMachineTrigger>();
        WeaponState();
    }
    void WeaponState()
    {
        IDisposable weaponState = _weaponAnimationTrigger
        .OnStateEnterAsObservable()　　//Animationの遷移開始を検知
        .Subscribe(onStateInfo =>
        {
            if (onStateInfo.StateInfo.IsTag("InDeformation"))
            {
                _inBoostMode = true;
            }
            else
            {
                _inBoostMode = false;
            }
        }).AddTo(this);
    }
    public void DeformationWeapon(WeaponType weaponType,ElementType elementType)
    {
        _targetWeapon.ObjectAnimator.SetInteger("WeaponType",(int)weaponType);
        _targetWeapon.ObjectAnimator.SetBool("IsOpen",elementType != ElementType.RIGIT);
        _targetWeapon.ObjectAnimator.SetTrigger("Transforming");
    }
}

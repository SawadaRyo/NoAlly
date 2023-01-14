using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    [Tooltip("変形前の武器の当たり判定")]
    protected Vector3 _normalHarfExtents = Vector3.zero;
    [Tooltip("変形後の武器の当たり判定")]
    protected Vector3 _pawerUpHarfExtents = Vector3.zero;
    [Tooltip("武器のアニメーション")]
    protected Animator _weaponAnimator = default;


    public override void Initialize(WeaponDataEntity weaponData)
    {
        base.Initialize(weaponData);
        _weaponAnimator = GetComponent<Animator>();
        _myParticleSystem.Stop();
    }
    //protected void BladeState()
    //{
    //    IDisposable exitState = _trigger
    //        .OnStateExitAsObservable()
    //        .Subscribe(onStateInfo =>
    //        {
    //            AnimatorStateInfo info = onStateInfo.StateInfo;
    //            if(_operated)
    //            {
    //                if (info.IsTag("Opening"))
    //                {
    //                    _isDeformated = true;
    //                    foreach (Renderer bR in _bladeRenderer)
    //                    {
    //                        BladeFadeOut(bR);
    //                    }
    //                }
    //                else if (info.IsTag("Closing"))
    //                {
    //                    _isDeformated = false;
    //                }
    //            }
    //        });
    //    IDisposable enterState = _trigger
    //        .OnStateEnterAsObservable()
    //        .Subscribe(onStateInfo =>
    //        {
    //            AnimatorStateInfo info = onStateInfo.StateInfo;
    //            if (_operated)
    //            {
    //                if (info.IsTag("Close"))
    //                {
    //                    foreach (Renderer bR in _bladeRenderer)
    //                    {
    //                        bR.enabled = false;
    //                    }
    //                }
    //            }
    //        });


    //}
    //protected void BladeFadeIn(Renderer bR)
    //{
    //    Color bRc = bR.material.color;
    //    DOTween.To(x => bRc.a = x
    //                , _fadeOutColor
    //                , _fadeInColor, _fadeTime)
    //                .OnUpdate(() => bR.material.color = bRc)
    //                .OnComplete(() => _weaponAnimator.SetBool("IsOpen", false));
    //}
    //void BladeFadeOut(Renderer bR)
    //{
    //    bR.enabled = true;
    //    Color bRc = bR.material.color;
    //    DOTween.To( x => bRc.a = x
    //                ,_fadeInColor
    //                ,_fadeOutColor, _fadeTime)
    //                .OnUpdate(() => bR.material.color = bRc);
    //}
}

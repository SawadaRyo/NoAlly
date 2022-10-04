using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    float _fadeInColor = 0f;
    float _fadeOutColor = 0.5f;
    float _fadeTime = 0.5f;
    ObservableStateMachineTrigger _trigger = default;


    [Tooltip("変形前の武器の当たり判定")]
    protected Vector3 _normalHarfExtents = Vector3.zero;
    [Tooltip("変形後の武器の当たり判定")]
    protected Vector3 _pawerUpHarfExtents = Vector3.zero;
    [Tooltip("武器のアニメーション")]
    protected Animator _weaponAnimator = default;
    [Tooltip("")]
    protected Renderer[] _bladeRenderer = default;

    public override void Awake()
    {
        _weaponAnimator = GetComponent<Animator>();
        if (_weaponAnimator != null)
        {
            _trigger = _weaponAnimator.GetBehaviour<ObservableStateMachineTrigger>();
            _bladeRenderer = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
            BladeState();
            foreach (Renderer bR in _bladeRenderer)
            {
                Color bRc = bR.material.color;
                bRc.a = 0;
                bR.material.color = bRc;
                bR.enabled = false;
            }
        }
        base.Awake();
        _myParticleSystem.Stop();
    }
    public override void RendererActive(bool stats)
    {
        base.RendererActive(stats);
        if (!_isDeformated) stats = false;
        foreach (Renderer bRs in _bladeRenderer)
        {
            bRs.enabled = stats;
        }
    }
    protected void BladeState()
    {
        IDisposable exitState = _trigger
            .OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if(_operated)
                {
                    if (info.IsTag("Opening"))
                    {
                        _isDeformated = true;
                        foreach (Renderer bR in _bladeRenderer)
                        {
                            BladeFadeOut(bR);
                        }
                    }
                    else if (info.IsTag("Closing"))
                    {
                        _isDeformated = false;
                    }
                }
            });
        IDisposable enterState = _trigger
            .OnStateEnterAsObservable()
            .Subscribe(onStateInfo =>
            {
                AnimatorStateInfo info = onStateInfo.StateInfo;
                if (_operated)
                {
                    if (info.IsTag("Close"))
                    {
                        foreach (Renderer bR in _bladeRenderer)
                        {
                            bR.enabled = false;
                        }
                    }
                }
            });


    }
    protected void BladeFadeIn(Renderer bR)
    {
        Color bRc = bR.material.color;
        DOTween.To(x => bRc.a = x
                    , _fadeOutColor
                    , _fadeInColor, _fadeTime)
                    .OnUpdate(() => bR.material.color = bRc)
                    .OnComplete(() => _weaponAnimator.SetBool("IsOpen", false));
    }
    void BladeFadeOut(Renderer bR)
    {
        bR.enabled = true;
        Color bRc = bR.material.color;
        DOTween.To( x => bRc.a = x
                    ,_fadeInColor
                    ,_fadeOutColor, _fadeTime).OnUpdate(() => bR.material.color = bRc);
    }
}

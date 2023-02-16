using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    [SerializeField,Tooltip("武器のアニメーション")]
    protected Animator _weaponAnimator = default;

    [Tooltip("変形前の武器の当たり判定")]
    protected Vector3 _normalHarfExtents = Vector3.zero;
    [Tooltip("変形後の武器の当たり判定")]
    protected Vector3 _pawerUpHarfExtents = Vector3.zero;


    public override void ActiveObject(bool stats)
    {
        base.ActiveObject(stats);
        Array.ForEach(_objectCollider, x => x.enabled = false);
    }
    public override void DisactiveForInstantiate()
    {
        base.DisactiveForInstantiate();
        if (_myParticleSystem != null)
        {
            _myParticleSystem.Stop();
        }
    }
}

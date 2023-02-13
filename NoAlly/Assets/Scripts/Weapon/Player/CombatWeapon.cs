using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    [Tooltip("�ό`�O�̕���̓����蔻��")]
    protected Vector3 _normalHarfExtents = Vector3.zero;
    [Tooltip("�ό`��̕���̓����蔻��")]
    protected Vector3 _pawerUpHarfExtents = Vector3.zero;
    [Tooltip("����̃A�j���[�V����")]
    protected Animator _weaponAnimator = default;


    public override void SetData(WeaponDataEntity weaponData)
    {
        base.SetData(weaponData);
        _weaponAnimator = GetComponent<Animator>();
        if (_myParticleSystem != null)
        {
            _myParticleSystem.Stop();
        }
    }
    public override void ActiveObject(bool stats)
    {
        base.ActiveObject(stats);
        Array.ForEach(_objectCollider, x => x.enabled = false);
    }
}

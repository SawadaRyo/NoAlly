using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CombatWeapon : WeaponBase
{
    [SerializeField,Tooltip("����̃A�j���[�V����")]
    protected Animator _weaponAnimator = default;

    [Tooltip("�ό`�O�̕���̓����蔻��")]
    protected Vector3 _normalHarfExtents = Vector3.zero;
    [Tooltip("�ό`��̕���̓����蔻��")]
    protected Vector3 _pawerUpHarfExtents = Vector3.zero;
}

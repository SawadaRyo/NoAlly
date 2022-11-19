using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [Tooltip("オブジェクトの生死判定")]
    protected bool _living = true;
    [SerializeField, Tooltip("敵のHPの上限")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("敵の物理耐性")]
    protected float _rigitDefensePercentage = 1f;
    [SerializeField, Tooltip("敵の炎耐性")]
    protected float _fireDifansePercentage = 1f;
    [SerializeField, Tooltip("敵の雷耐性")]
    protected float _elekeDifansePercentage = 1f;
    [SerializeField, Tooltip("敵の氷結耐性")]
    protected float _frozenDifansePercentage = 1f;

    [Tooltip("オブジェクトの現在のHP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("無敵時間")]
    protected Interval _invincibleTime = new Interval(0.4f);
    [Tooltip("AudioSourceを格納する変数")]
    protected AudioSource _audioSource = null;
    [SerializeField, Tooltip("ダメージサウンド")]
    protected AudioClip _damageSound;
    [Tooltip("Animatorの格納変数")]
    protected Animator _animator;

    public bool Living => _living;
    public bool IsInvincible => _invincibleTime.IsCountUp();
    public FloatReactiveProperty HP => _hp;

    public virtual void Init()
    {
        _hp = new FloatReactiveProperty(_maxHP);
        _living = true;
        _animator = GetComponentInParent<Animator>();
    }

}

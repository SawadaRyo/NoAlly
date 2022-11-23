using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StatusBase : MonoBehaviour
{
    [Tooltip("�I�u�W�F�N�g�̐�������")]
    protected bool _living = true;
    [SerializeField, Tooltip("�G��HP�̏��")]
    protected float _maxHP = 5;
    [SerializeField, Tooltip("�G�̕����ϐ�")]
    protected float _rigitDefensePercentage = 1f;
    [SerializeField, Tooltip("�G�̉��ϐ�")]
    protected float _fireDifansePercentage = 1f;
    [SerializeField, Tooltip("�G�̗��ϐ�")]
    protected float _elekeDifansePercentage = 1f;
    [SerializeField, Tooltip("�G�̕X���ϐ�")]
    protected float _frozenDifansePercentage = 1f;

    [Tooltip("�I�u�W�F�N�g�̌��݂�HP")]
    protected FloatReactiveProperty _hp;
    [Tooltip("���G����")]
    protected Interval _invincibleTime = new Interval(0.4f);
    [Tooltip("AudioSource���i�[����ϐ�")]
    protected AudioSource _audioSource = null;
    [SerializeField, Tooltip("�_���[�W�T�E���h")]
    protected AudioClip _damageSound;
    [Tooltip("Animator�̊i�[�ϐ�")]
    protected Animator _animator;

    public bool Living => _living;
    public bool IsInvincible => _invincibleTime.IsCountUp();
    public FloatReactiveProperty HP => _hp;

    private void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        _hp = new FloatReactiveProperty(_maxHP);
        _living = true;
        _animator = GetComponentInParent<Animator>();
    }

}

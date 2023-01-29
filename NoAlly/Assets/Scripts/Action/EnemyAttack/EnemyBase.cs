using System;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : ObjectVisual, IObjectPool
{
    [SerializeField, Header("���G�͈�")]
    protected float _radius = 5f;
    [SerializeField, Header("���G�p�̃��C���[")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("���G�͈͂̒��S")]
    protected Transform _center = default;

    [Tooltip("���̃I�u�W�F�N�g�̐�������")]
    bool _isActive = true;
    [Tooltip("�X�e�[�g�}�V��")]
    protected StateMachine<EnemyBase> _stateMachine = null;


    /// <summary>
    /// ���̃I�u�W�F�N�g�̐�������̃v���p�e�B(�ǂݎ���p)
    /// </summary>
    public bool IsActive => _isActive;
    public Animator EnemyAnimator => _objectAnimator;
    public PlayerStatus Player => InSight();
    /// <summary>
    /// �X�e�[�g�}�V�[���̃I�[�i�[(����)��Ԃ��v���p�e�B(�ǂݎ���p)
    /// </summary>
    public StateMachine<EnemyBase> EnemyStateMachine => _stateMachine;

    public abstract void EnemyAttack();
    public virtual void Start()
    {

    }
    public void FixedUpdate()
    {
        if (_isActive)
        {
            _stateMachine.Update();
        }
    }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }

    public PlayerStatus InSight()
    {
        Collider[] inSight = Physics.OverlapSphere(_center.position, _radius, _playerLayer);
        foreach (var s in inSight)
        {
            if (s.gameObject.TryGetComponent<PlayerStatus>(out PlayerStatus player))
            {
                return player;
            }
        }
        return null;
    }
    /// <summary>
    /// �I�u�W�F�N�g�L�����ɌĂԊ֐�
    /// </summary>
    public virtual void Create()
    {
        _isActive = true;
        ActiveObject(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// �I�u�W�F�N�g��L�����ɌĂԊ֐�
    /// </summary>
    public virtual void Disactive()
    {
        _isActive = false;
        ActiveObject(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// �I�u�W�F�N�g�������ɌĂԊ֐�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Owner"></param>
    public virtual void DisactiveForInstantiate<T>(T Owner) where T : IObjectGenerator
    {
        _isActive = false;
        ActiveObject(_isActive);
        _stateMachine = new StateMachine<EnemyBase>(this);
        {
            //�v���C���[�����������v���C���[���U��
            _stateMachine.AddTransition<Search, Attack>((int)EnemyState.Attack);
            //�v���C���[�����������Ƃ��U���𒆎~
            _stateMachine.AddTransition<Attack,Search>((int)EnemyState.Saerching);
            //HP��0�ɂȂ����Ƃ����S
            _stateMachine.AddAnyTransition<Death>((int)EnemyState.Death);
            _stateMachine.Start<Search>();
        }


    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
    protected enum EnemyState : int
    {
        None,
        Saerching,
        Attack,
        Death
    }
}
public class Search : State
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.InSight())
        {
            Owner.EnemyStateMachine.Dispatch((int)EnemyState.Attack);
        }
    }
}
public class Attack : State
{
    public virtual void AttackBehaviour() { }
    public virtual void Initalize<TEnemy>(TEnemy enemy) where TEnemy : EnemyBase { }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        AttackBehaviour();
    }
}
public class Death : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.Disactive();
    }
}
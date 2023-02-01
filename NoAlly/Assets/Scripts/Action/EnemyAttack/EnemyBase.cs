using System;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : ObjectVisual
{
    [SerializeField, Header("���G�͈�")]
    protected float _radius = 5f;
    [SerializeField, Header("���G�p�̃��C���[")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("���G�͈͂̒��S")]
    protected Transform _center = default;

    [Tooltip("�X�e�[�g�}�V��")]
    protected StateMachine<EnemyBase> _stateMachine = null;

    public Animator EnemyAnimator => _objectAnimator;
    public PlayerStatus Player => InSight();
    /// <summary>
    /// �X�e�[�g�}�V�[���̃I�[�i�[(����)��Ԃ��v���p�e�B(�ǂݎ���p)
    /// </summary>
    public StateMachine<EnemyBase> EnemyStateMachine => _stateMachine;

    public abstract void EnemyAttack();
    public virtual void Start() { }
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
            if (s.gameObject.TryGetComponent(out PlayerStatus player))
            {
                return player;
            }
        }
        return null;
    }
    /// <summary>
    /// �I�u�W�F�N�g�L�����ɌĂԊ֐�
    /// </summary>
    public override void Create()
    {
        base.Create();
        _objectAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// �I�u�W�F�N�g��L�����ɌĂԊ֐�
    /// </summary>
    public override void Disactive()
    {
        base.Disactive();
        _objectAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// �I�u�W�F�N�g�������ɌĂԊ֐�
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <param name="Owner"></param>
    public override void DisactiveForInstantiate<TOwner>(TOwner Owner)
    {
        base.DisactiveForInstantiate(Owner);
        _stateMachine = new StateMachine<EnemyBase>(this);
        {
            //HP��0�ɂȂ����Ƃ����S
            _stateMachine.AddAnyTransition<Death>((int)StateOfEnemy.Death);
            _stateMachine.Start<Search>();
        }
    }


    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
}
public class Search : State
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.Player)
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Attack);
        }
    }
}
public class Attack : State
{
    public virtual void AttackBehaviour() { }
}
public class Death : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.Disactive();
    }
}
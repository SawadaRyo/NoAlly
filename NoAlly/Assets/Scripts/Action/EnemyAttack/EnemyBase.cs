using System;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : ObjectVisual
{
    [SerializeField, Header("索敵範囲")]
    protected float _radius = 5f;
    [SerializeField, Header("索敵用のレイヤー")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("索敵範囲の中心")]
    protected Transform _center = default;

    [Tooltip("ステートマシン")]
    protected StateMachine<EnemyBase> _stateMachine = null;

    public Animator EnemyAnimator => _objectAnimator;
    public PlayerStatus Player => InSight();
    /// <summary>
    /// ステートマシーンのオーナー(自分)を返すプロパティ(読み取り専用)
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
    /// オブジェクト有効時に呼ぶ関数
    /// </summary>
    public override void Create()
    {
        base.Create();
        _objectAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// オブジェクト非有効時に呼ぶ関数
    /// </summary>
    public override void Disactive()
    {
        base.Disactive();
        _objectAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// オブジェクト生成時に呼ぶ関数
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <param name="Owner"></param>
    public override void DisactiveForInstantiate<TOwner>(TOwner Owner)
    {
        base.DisactiveForInstantiate(Owner);
        _stateMachine = new StateMachine<EnemyBase>(this);
        {
            //HPが0になったとき死亡
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
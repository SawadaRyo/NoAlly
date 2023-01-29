using System;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : ObjectVisual, IObjectPool
{
    [SerializeField, Header("索敵範囲")]
    protected float _radius = 5f;
    [SerializeField, Header("索敵用のレイヤー")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("索敵範囲の中心")]
    protected Transform _center = default;

    [Tooltip("このオブジェクトの生死判定")]
    bool _isActive = true;
    [Tooltip("ステートマシン")]
    protected StateMachine<EnemyBase> _stateMachine = null;


    /// <summary>
    /// このオブジェクトの生死判定のプロパティ(読み取り専用)
    /// </summary>
    public bool IsActive => _isActive;
    public Animator EnemyAnimator => _objectAnimator;
    public PlayerStatus Player => InSight();
    /// <summary>
    /// ステートマシーンのオーナー(自分)を返すプロパティ(読み取り専用)
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
    /// オブジェクト有効時に呼ぶ関数
    /// </summary>
    public virtual void Create()
    {
        _isActive = true;
        ActiveObject(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// オブジェクト非有効時に呼ぶ関数
    /// </summary>
    public virtual void Disactive()
    {
        _isActive = false;
        ActiveObject(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// オブジェクト生成時に呼ぶ関数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Owner"></param>
    public virtual void DisactiveForInstantiate<T>(T Owner) where T : IObjectGenerator
    {
        _isActive = false;
        ActiveObject(_isActive);
        _stateMachine = new StateMachine<EnemyBase>(this);
        {
            //プレイヤーを見つけた時プレイヤーを攻撃
            _stateMachine.AddTransition<Search, Attack>((int)EnemyState.Attack);
            //プレイヤーを見失ったとき攻撃を中止
            _stateMachine.AddTransition<Attack,Search>((int)EnemyState.Saerching);
            //HPが0になったとき死亡
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
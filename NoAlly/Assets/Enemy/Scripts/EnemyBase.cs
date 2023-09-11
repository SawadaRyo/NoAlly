using System;
using UnityEngine;
using UniRx;

public abstract class EnemyBase : ObjectBase, IObjectPool<IObjectGenerator>
{
    [SerializeField, Header("")]
    protected EnemyParamaterBase _enemyParamater = null;
    protected Rigidbody _rb = null;
    [SerializeField, Header("索敵範囲の中心")]
    protected Transform _center = default;

    [Tooltip("ステートマシン")]
    protected StateMachine<EnemyBase> _stateMachine = null;
    [Tooltip("")]
    ReactiveProperty<PlayerStatus> _playerStatus = new();

    public IReadOnlyReactiveProperty<PlayerStatus> Player => _playerStatus;
    /// <summary>
    /// ステートマシーンのオーナー(自分)を返すプロパティ(読み取り専用)
    /// </summary>
    public StateMachine<EnemyBase> EnemyStateMachine => _stateMachine;

    public IObjectGenerator Owner => throw new NotImplementedException();

    public virtual void EnemyAttack() { }
    public virtual void ExitAttackState() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }

    public PlayerStatus InSight()
    {
        Collider[] inSight = Physics.OverlapSphere(_center.position, _enemyParamater.searchRenge, _enemyParamater.targetLayer);
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
    public virtual void Create()
    {
        _isActive = true;
        ActiveCollider(_isActive);
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
    /// オブジェクト非有効時に呼ぶ関数(インターバル有)
    /// </summary>
    public virtual void Disactive(float interval) { }
    /// <summary>
    /// オブジェクト生成時に呼ぶ関数
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <param name="Owner"></param>
    public virtual void DisactiveForInstantiate(IObjectGenerator Owner)
    {
        _isActive = false;
        _stateMachine = new StateMachine<EnemyBase>(this);
        OnUpdate();
    }
    void OnUpdate()
    {
        Observable.EveryFixedUpdate()
            .Where(_ => _isActive == true)
            .Subscribe(_ =>
            {
                _playerStatus.Value = InSight();
                _stateMachine.Update();
            });
    }

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _enemyParamater.searchRenge);
    }

#endif
}


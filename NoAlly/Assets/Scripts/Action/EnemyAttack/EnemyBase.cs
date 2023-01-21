using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : MonoBehaviour, IObjectPool
{
    [SerializeField, Header("索敵範囲")]
    protected float _radius = 5f;
    [SerializeField, Header("このオブジェクトのアニメーター")]
    protected Animator _enemyAnimator;
    [SerializeField, Header("索敵用のレイヤー")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("索敵範囲の中心")]
    protected Transform _center = default;
    [SerializeField, Header("このオブジェクトのObjectVisual")]
    ObjectVisual _thisObject = default;

    [Tooltip("このオブジェクトの生死判定")]
    bool _isActive = true;
    [Tooltip("ステートマシン")]
    StateMachine<EnemyBase> _stateMachine = null;

    /// <summary>
    /// このオブジェクトの生死判定のプロパティ(読み取り専用)
    /// </summary>
    public bool IsActive => _isActive;
    public float Radius => _radius;
    public Transform Center => _center;
    public Animator EnemyAnimator => _enemyAnimator;
    protected int EnemyCurrentState<TState>(TState state) where int => 

    public abstract void EnemyAttack();
    public virtual void Start()
    {

    }
    public void FixedUpdate()
    {
        if (_isActive)
        {
            EnemyAttack();
        }
    }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }

    protected PlayerContoller InSight()
    {
        //_center = transform.position;
        Collider[] inSight = Physics.OverlapSphere(_center.position, _radius, _playerLayer);
        foreach (var s in inSight)
        {
            if (s.gameObject.TryGetComponent<PlayerContoller>(out PlayerContoller player))
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
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// オブジェクト非有効時に呼ぶ関数
    /// </summary>
    public virtual void Disactive()
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// オブジェクト生成時に呼ぶ関数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Owner"></param>
    public virtual void DisactiveForInstantiate<T>(T Owner) where T : IObjectGenerator
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
        _stateMachine = new StateMachine<EnemyBase>(this);
        //HPが0になったとき死亡
        _stateMachine.AddAnyTransition<Death>((int)EnemyState.Death);

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

class Death : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.Disactive();
    }
}



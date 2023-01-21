using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : MonoBehaviour, IObjectPool
{
    [SerializeField, Header("���G�͈�")]
    protected float _radius = 5f;
    [SerializeField, Header("���̃I�u�W�F�N�g�̃A�j���[�^�[")]
    protected Animator _enemyAnimator;
    [SerializeField, Header("���G�p�̃��C���[")]
    protected LayerMask _playerLayer = ~0;
    [SerializeField, Header("���G�͈͂̒��S")]
    protected Transform _center = default;
    [SerializeField, Header("���̃I�u�W�F�N�g��ObjectVisual")]
    ObjectVisual _thisObject = default;

    [Tooltip("���̃I�u�W�F�N�g�̐�������")]
    bool _isActive = true;
    [Tooltip("�X�e�[�g�}�V��")]
    StateMachine<EnemyBase> _stateMachine = null;

    /// <summary>
    /// ���̃I�u�W�F�N�g�̐�������̃v���p�e�B(�ǂݎ���p)
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
    /// �I�u�W�F�N�g�L�����ɌĂԊ֐�
    /// </summary>
    public virtual void Create()
    {
        _isActive = true;
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// �I�u�W�F�N�g��L�����ɌĂԊ֐�
    /// </summary>
    public virtual void Disactive()
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// �I�u�W�F�N�g�������ɌĂԊ֐�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Owner"></param>
    public virtual void DisactiveForInstantiate<T>(T Owner) where T : IObjectGenerator
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
        _stateMachine = new StateMachine<EnemyBase>(this);
        //HP��0�ɂȂ����Ƃ����S
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



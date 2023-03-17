using System;
using UnityEngine;
using DG.Tweening;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyBase : ObjectBase,IObjectPool<IObjectGenerator>
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

    public IObjectGenerator Owner => throw new NotImplementedException();

    public virtual void EnemyAttack() { }
    public virtual void EnemyRotate(Transform playerPos) { }
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
    public virtual void Create()
    {
        _isActive = true;
        ActiveCollider(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);
    }
    /// <summary>
    /// �I�u�W�F�N�g��L�����ɌĂԊ֐�
    /// </summary>
    public virtual void Disactive()
    {
        _isActive = false;
        ActiveCollider(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);

    }
    public virtual void Disactive(float interval)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// �I�u�W�F�N�g�������ɌĂԊ֐�
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    /// <param name="Owner"></param>
    public void DisactiveForInstantiate(IObjectGenerator Owner)
    {
        _isActive = false;
        _stateMachine = new StateMachine<EnemyBase>(this);
        {
            //�v���C���[�����������v���C���[���U��
            _stateMachine.AddTransition<Search, Attack>((int)StateOfEnemy.Attack);
            //�v���C���[�����������Ƃ��U���𒆎~
            _stateMachine.AddTransition<Attack, Search>((int)StateOfEnemy.Saerching);
            //HP��0�ɂȂ����Ƃ����S
            _stateMachine.AddAnyTransition<Death>((int)StateOfEnemy.Death);
            _stateMachine.Start<Search>();
        }
    }

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }

#endif
}
public class Search : State
{
    bool _rotated;
    float _time = 0;
    float _intervalRotate = 3;
    float _turnSpeed = 10f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(!Owner.Player)
        {
            _time += Time.deltaTime;
            if(_time >= _intervalRotate)
            {
                _rotated = !_rotated;
                if (_rotated)
                {
                    Owner.transform.DORotate(new Vector3(0f,-90f,0f),0.5f);
                }
                else
                {
                    Owner.transform.DORotate(new Vector3(0f, 90f, 0f), 0.5f);
                }
                _time = 0;
            }
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Attack);
        }
    }
}
public class Attack : State
{
    protected override void OnEnter(StateMachine<EnemyBase>.State prevState)
    {
        base.OnEnter(prevState);
        Owner.EnemyAnimator.SetBool("Aiming", true);
        //Owner.StartCoroutine(RapidFire((GunTypeEnemy)Owner));
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(Owner.Player)
        {
            Owner.EnemyAttack();
            Owner.EnemyRotate(Owner.Player.transform);
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Saerching);
        }
    }
    protected override void OnExit(StateMachine<EnemyBase>.State nextState)
    {
        base.OnExit(nextState);
        Owner.EnemyAnimator.SetBool("Aiming", false);
    }
}
public class Death : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetBool("Death", true);
    }
}


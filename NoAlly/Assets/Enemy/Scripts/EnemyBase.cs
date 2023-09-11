using System;
using UnityEngine;
using UniRx;

public abstract class EnemyBase : ObjectBase, IObjectPool<IObjectGenerator>
{
    [SerializeField, Header("")]
    protected EnemyParamaterBase _enemyParamater = null;
    protected Rigidbody _rb = null;
    [SerializeField, Header("���G�͈͂̒��S")]
    protected Transform _center = default;

    [Tooltip("�X�e�[�g�}�V��")]
    protected StateMachine<EnemyBase> _stateMachine = null;
    [Tooltip("")]
    ReactiveProperty<PlayerStatus> _playerStatus = new();

    public IReadOnlyReactiveProperty<PlayerStatus> Player => _playerStatus;
    /// <summary>
    /// �X�e�[�g�}�V�[���̃I�[�i�[(����)��Ԃ��v���p�e�B(�ǂݎ���p)
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
        ActiveObject(_isActive);
        _objectAnimator.SetBool("Death", !_isActive);

    }
    /// <summary>
    /// �I�u�W�F�N�g��L�����ɌĂԊ֐�(�C���^�[�o���L)
    /// </summary>
    public virtual void Disactive(float interval) { }
    /// <summary>
    /// �I�u�W�F�N�g�������ɌĂԊ֐�
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


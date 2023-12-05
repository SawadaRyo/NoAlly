using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField, Tooltip("�����x")]
    float _moveMagnification = 2f;
    [SerializeField, Tooltip("�U���͈�")]
    float _attackRadius = 1.6f;
    [SerializeField, Tooltip("�U���͈͂̒��S")]
    Transform _attackPos = default;
    [SerializeField, Tooltip("")]
    LayerMask _fieldLayer = ~0;

    [Tooltip("")]
    Vector2 _moveVec = Vector2.zero;
    [Tooltip("")]
    bool _hit = false;
    [Tooltip("")]
    float _currentSpeed = 0f;
    float _time = 0f;

    public override void ExitAttackState()
    {
        base.ExitAttackState();
        _hit = false;
        _currentSpeed = 0f;
        _time = 0f;
        _rb.velocity = _moveVec.normalized * _currentSpeed;
    }

    public override void Disactive()
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
    }
    public override void DisactiveForInstantiate(IObjectGenerator Owner)
    {
        base.DisactiveForInstantiate(Owner);
        //�퓬�Ԑ�
        _stateMachine.AddTransition<EnemySearch, UAVBattlePosture>((int)StateOfEnemy.BattlePosture);
        //�퓬�Ԑ�����
        _stateMachine.AddTransition<UAVBattlePosture, EnemySearch>((int)StateOfEnemy.Saerching);
        //�v���C���[�ւ̍U��
        _stateMachine.AddTransition<UAVBattlePosture, UAVAttack>((int)StateOfEnemy.Attack);
        //���S
        _stateMachine.AddAnyTransition<EnemyDeath>((int)StateOfEnemy.Death);
        //�X�e�[�g�J�n
        _stateMachine.Start<EnemySearch>();
    }

    private void ResetParamater()
    {
        
    }
}

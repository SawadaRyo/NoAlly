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

    public override void EnemyAttack()
    {
        //if (InSight())
        //{
        //    var targetPos = Player.Value.transform.position + new Vector3(0f, 1.8f, 0f);
        //    transform.LookAt(targetPos);
        //    _moveVec = (targetPos - transform.position);
        //    if (_hit)
        //    {
        //        _currentSpeed = (-_enemyParamater.speed * _moveMagnification);
        //        _time += Time.deltaTime;
        //        if (_time > 1f || !InSight())
        //        {
        //            _hit = false;
        //            _time = 0f;
        //        }
        //    }
        //    else
        //    {
        //        _currentSpeed = _enemyParamater.speed;
        //        IHitBehavorOfAttack playerStatus = CallPlayerGauge();
        //        if (playerStatus != null)
        //        {
        //            playerStatus.BehaviorOfHit(_enemyParamater.enemyPowers[(int)ElementType.ELEKE], ElementType.ELEKE);
        //            _hit = true;
        //        }
        //    }
        //}
        //_rb.velocity = _moveVec.normalized * _currentSpeed;
    }
    public override void ExitAttackState()
    {
        base.ExitAttackState();
        _hit = false;
        _currentSpeed = 0f;
        _time = 0f;
        _rb.velocity = _moveVec.normalized * _currentSpeed;
    }
    IHitBehavorOfAttack CallPlayerGauge()
    {
        Collider[] playerCol = Physics.OverlapSphere(_attackPos.position, _attackRadius, _enemyParamater.targetLayer);
        foreach (Collider col in playerCol)
        {
            if (col.TryGetComponent(out IHitBehavorOfAttack playerGauge))
            {
                return playerGauge;
            }
        }
        return null;
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
}

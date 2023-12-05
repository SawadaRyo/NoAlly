using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField, Tooltip("加速度")]
    float _moveMagnification = 2f;
    [SerializeField, Tooltip("攻撃範囲")]
    float _attackRadius = 1.6f;
    [SerializeField, Tooltip("攻撃範囲の中心")]
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
        //戦闘態勢
        _stateMachine.AddTransition<EnemySearch, UAVBattlePosture>((int)StateOfEnemy.BattlePosture);
        //戦闘態勢解除
        _stateMachine.AddTransition<UAVBattlePosture, EnemySearch>((int)StateOfEnemy.Saerching);
        //プレイヤーへの攻撃
        _stateMachine.AddTransition<UAVBattlePosture, UAVAttack>((int)StateOfEnemy.Attack);
        //死亡
        _stateMachine.AddAnyTransition<EnemyDeath>((int)StateOfEnemy.Death);
        //ステート開始
        _stateMachine.Start<EnemySearch>();
    }

    private void ResetParamater()
    {
        
    }
}

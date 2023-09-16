//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public abstract class EnemyMove : State
{
    protected abstract void MoveBehaviour();
    public virtual void EnemyRotate(Transform playerPos) { }

    protected override void OnEnter(State prevState)
    {

    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        EnemyRotate(Owner.Player.Value.transform);
    }
    protected override void OnExit(State nextState)
    {
        
    }
}

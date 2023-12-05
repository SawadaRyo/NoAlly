//日本語コメント可
using State = StateMachine<EnemyBase>.State;
using System;

public class UAVAttack : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetTrigger("");
    }


    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }
}

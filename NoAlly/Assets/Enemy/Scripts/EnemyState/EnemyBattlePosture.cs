//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public class EnemyBattlePosture : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetBool("InSight", true);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.Player.Value)
        {
            
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Saerching);
        }
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        Owner.ObjectAnimator.SetBool("InSight", false);
        Owner.ExitAttackState();
    }
}

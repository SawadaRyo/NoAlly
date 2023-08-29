//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<EnemyBase>.State;

public class UAVAttack : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetTrigger("");
    }
}

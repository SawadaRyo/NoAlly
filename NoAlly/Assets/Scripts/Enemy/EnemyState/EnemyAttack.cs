using System;
using UnityEngine;
using DG.Tweening;
using State = StateMachine<EnemyBase>.State;

public class EnemyAttack : State
{
    protected virtual void AttackBehaviour() { }
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
            AttackBehaviour();
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
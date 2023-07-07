using System;
using UnityEngine;
using DG.Tweening;
using State = StateMachine<EnemyBase>.State;

public class EnemyAttack : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.EnemyAnimator.SetBool("InSight", true);
        //Owner.StartCoroutine(RapidFire((GunTypeEnemy)Owner));
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.Player)
        {
            Owner.EnemyAttack();
            Owner.EnemyRotate(Owner.Player.transform);
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Saerching);
        }
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        Owner.EnemyAnimator.SetBool("InSight", false);
        Owner.ExitAttackState();
    }
}
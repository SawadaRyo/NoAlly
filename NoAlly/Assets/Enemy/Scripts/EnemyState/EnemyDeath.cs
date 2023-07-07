using System;
using UnityEngine;
using DG.Tweening;
using State = StateMachine<EnemyBase>.State;

public class EnemyDeath : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetBool("Death", true);
    }
}
using System;
using UnityEngine;
using DG.Tweening;
using UniRx;
using State = StateMachine<EnemyBase>.State;

public class EnemySearch : State
{
    bool _rotated;
    float _time = 0;
    float _intervalRotate = 3;
    float _turnDuration = 0.5f;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!Owner.Player.Value)
        {
            _time += Time.deltaTime;
            if (_time >= _intervalRotate)
            {
                _rotated = !_rotated;
                if (_rotated)
                {
                    Owner.transform.DORotate(new Vector3(0f, -90f, 0f), _turnDuration);
                }
                else
                {
                    Owner.transform.DORotate(new Vector3(0f, 90f, 0f), _turnDuration);
                }
                _time = 0;
            }
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.BattlePosture);
        }
    }

    protected void SearchBehaviour() { }

    protected override void OnTranstion()
    {
        base.OnTranstion();
        Owner.Player
            .Subscribe(player =>
            {

            });
    }
}

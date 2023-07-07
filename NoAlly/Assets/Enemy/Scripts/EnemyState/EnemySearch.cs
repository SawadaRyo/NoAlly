using System;
using UnityEngine;
using DG.Tweening;
using State = StateMachine<EnemyBase>.State;

public class EnemySearch : State
{
    bool _rotated;
    float _time = 0;
    float _intervalRotate = 3;
    float _turnSpeed = 10f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!Owner.Player)
        {
            _time += Time.deltaTime;
            if (_time >= _intervalRotate)
            {
                _rotated = !_rotated;
                if (_rotated)
                {
                    Owner.transform.DORotate(new Vector3(0f, -90f, 0f), 0.5f);
                }
                else
                {
                    Owner.transform.DORotate(new Vector3(0f, 90f, 0f), 0.5f);
                }
                _time = 0;
            }
        }
        else
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Attack);
        }
    }
}

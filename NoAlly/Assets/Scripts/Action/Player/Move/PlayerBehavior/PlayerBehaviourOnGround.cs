//日本語コメント可
using ActorBehaviour;
using State = StateMachine<PlayerMoveInput>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviourOnGround : State
{
    Vector3 _velo = Vector3.zero;

    public Vector3 Velo => _velo;

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.CurrentMoveVector.Value != Vector2.zero)
        {
            Owner.Rb.velocity =
                ActorMove.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.Rb, Owner.HitInfo.normal);
            //Debug.Log(_rb.velocity);
            _velo = Owner.Rb.velocity;
        }
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        if(nextState is PlayerBehaviorInAir air)
        {
            air.BeforeMoveVec = _velo;
        }
    }

    public override void OnTranstion()
    {
        Debug.Log(Owner);
        Owner.CurrentLocation
            .Subscribe(currentLocation =>
            {
                switch (currentLocation)
                {
                    case StateOfPlayer.InAir:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
                        break;
                    case StateOfPlayer.GripingWall:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.GripingWall);
                        break;
                    default:
                        break;
                }
            }).AddTo(Owner);
        Owner.IsDash
            .Where(_ => Owner.IsDash.Value == true)
            .Subscribe(isDash =>
            {
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.Dash);
            });
    }
}

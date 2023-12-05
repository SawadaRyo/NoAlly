//日本語コメント可
using State = StateMachine<PlayerBehaviorController>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorDash : State
{
    float _veloX = 0f;

    public float VeloX => _veloX;

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        _veloX = 0f;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.MoveBehaviour.ActorRotateMethod(Owner.ParamaterCon.GetParamater.turnSpeed, Owner.transform, Owner.CurrentMoveVector.Value);
        var moveVec = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.ParamaterCon.GetParamater.speed, Owner.HitInfo);
        Owner.Rb.velocity = moveVec + Owner.MoveBehaviour.DodgeVec(moveVec.normalized, Owner.ParamaterCon.GetParamater.dashSpeed);
        _veloX = Owner.ParamaterCon.GetParamater.speed + Owner.ParamaterCon.GetParamater.dashSpeed;
        if(Owner.AbleDash)
        {
            Owner.StateMachinePlayerMove.Dispatch((int)Owner.CurrentLocation.Value);
        }
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        if (nextState is PlayerBehaviorInAir air)
        {
            air.MoveSpeedX = _veloX;
        }
    }

    protected override void OnTranstion()
    {
        Owner.CurrentLocation
           .Skip(1)
           .Where(_ => IsActive)
           .Subscribe(currentLocation =>
           {
               Owner.StateMachinePlayerMove.Dispatch((int)currentLocation);
           }).AddTo(Owner);
        Owner.CurrentMoveVector
            .Skip(1)
            .Where(_ => Owner.CurrentMoveVector.Value == Vector2.zero)
            .Subscribe(moveVector =>
            {
                Owner.StateMachinePlayerMove.Dispatch((int)Owner.CurrentLocation.Value);
            });
        Owner.IsJump
            .Where(_ => Owner.IsJump.Value && IsActive && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isjump =>
            {
                Owner.Rb.velocity = new Vector3(_veloX, Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.jumpPower).y);
                Owner.StateMachinePlayerMove.Dispatch((int)StateOfPlayer.InAir);
            });
    }
}

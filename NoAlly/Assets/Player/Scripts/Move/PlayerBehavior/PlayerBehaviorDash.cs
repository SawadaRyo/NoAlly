//日本語コメント可
using State = StateMachine<PlayerBehaviorController>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorDash : State
{
    FloatReactiveProperty _time = new();
    float _veloX = 0f;

    public float VeloX => _veloX;

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        _time.Value = Owner.ParamaterCon.GetParamater.dashIntarval;
        _veloX = 0f;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        var moveVec = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.ParamaterCon.GetParamater.speed, Owner.HitInfo);
        Owner.Rb.velocity = moveVec + Owner.MoveBehaviour.DodgeVec(moveVec.normalized, Owner.ParamaterCon.GetParamater.dashSpeed);
        _veloX = Owner.ParamaterCon.GetParamater.speed + Owner.ParamaterCon.GetParamater.dashSpeed;
        _time.Value -= Time.deltaTime;
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        _time.Value = 0f;
        if (nextState is PlayerBehaviorInAir air)
        {
            air.MoveSpeedX = _veloX;
        }
        else if(nextState is PlayerBehaviourOnWall wall)
        {
            wall.MoveSpeedX = _veloX;
        }
    }

    public override void OnTranstion()
    {
        Owner.CurrentLocation
           .Skip(1)
           .Where(_ => IsActive)
           .Subscribe(currentLocation =>
           {
               Owner.PlayerStateMachine.Dispatch((int)currentLocation);
           }).AddTo(Owner);
        _time
            .Where(_ => IsActive)
            .Subscribe(time =>
            {
                if (time < 0f)
                {
                    Owner.PlayerStateMachine.Dispatch((int)Owner.CurrentLocation.Value);
                }
            });
        Owner.CurrentMoveVector
            .Skip(1)
            .Where(_ => Owner.CurrentMoveVector.Value == Vector2.zero)
            .Subscribe(moveVector =>
            {
                Owner.PlayerStateMachine.Dispatch((int)Owner.CurrentLocation.Value);
            });
        Owner.IsJump
            .Where(_ => Owner.IsJump.Value && IsActive && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isjump =>
            {
                Owner.Rb.velocity = new Vector3(_veloX, Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.jumpPower).y);
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
            });
    }

    ~PlayerBehaviorDash()
    {
        _time.Dispose();
    }
}

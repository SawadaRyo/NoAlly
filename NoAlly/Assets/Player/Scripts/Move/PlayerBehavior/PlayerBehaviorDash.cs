//日本語コメント可
using ActorBehaviour;
using State = StateMachine<PlayerMoveInput>.State;
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
        _time.Value = Owner.PlayerParamater.dashInterval;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        var moveVec = ActorMove.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.Rb, Owner.HitInfo.normal);
        Owner.Rb.velocity = moveVec + ActorMove.DodgeVec(moveVec.normalized, Owner.PlayerParamater.dashSpeed);
        Debug.Log(Owner.Rb.velocity);
        _veloX = Owner.CurrentMoveVector.Value.x * (Owner.PlayerParamater.speed + Owner.PlayerParamater.dashSpeed);
        _time.Value -= Time.deltaTime;
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        _time.Value = 0f;
        if (nextState is PlayerBehaviorInAir air)
        {
            air.BeforeMoveVec = new Vector3(_veloX, 0f, 0f);
        }
    }

    public override void OnTranstion()
    {
        Owner.CurrentLocation
           .Skip(1)
           .Subscribe(currentLocation =>
           {
               Owner.PlayerStateMachine.Dispatch((int)currentLocation);
           }).AddTo(Owner);
        _time
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
    }

    ~PlayerBehaviorDash()
    {
        _time.Dispose();
    }
}

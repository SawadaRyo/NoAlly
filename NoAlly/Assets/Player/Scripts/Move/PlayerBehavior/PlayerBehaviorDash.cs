//日本語コメント可
using ActorBehaviour;
using State = StateMachine<PlayerMoveInput>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorDash : State
{
    FloatReactiveProperty _time = new();
    Vector3 _velo = Vector3.zero;

    public Vector3 Velo => _velo;

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
        _time.Value -= Time.deltaTime;
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        _time.Value = 0f;
        if (nextState is PlayerBehaviorInAir air)
        {
            air.BeforeMoveVec = _velo;
        }
    }

    public override void OnTranstion()
    {
        _time
            .Subscribe(time =>
            {
                if (time < 0f)
                {
                    Owner.PlayerStateMachine.Dispatch((int)Owner.CurrentLocation.Value);
                }
            });
        Owner.CurrentLocation
           .Skip(1)
           .Subscribe(currentLocation =>
           {
               Owner.PlayerStateMachine.Dispatch((int)currentLocation);
           }).AddTo(Owner);
    }

    ~PlayerBehaviorDash()
    {
        _time.Dispose();
    }
}

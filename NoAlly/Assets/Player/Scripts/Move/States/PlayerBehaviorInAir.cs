//日本語コメント可
using State = StateMachine<PlayerBehaviorController>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorInAir : State
{
    float _moveSpeedX = 0f;
    RaycastHit _hitWall;

    public RaycastHit HitWall => _hitWall;
    public float MoveSpeedX { get => _moveSpeedX; set => _moveSpeedX = value; }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        float x = 0f;
        float y = 0f;
        if (Owner.AbleMove)
        {
            Owner.MoveBehaviour.ActorRotateMethod(Owner.ParamaterCon.GetParamater.turnSpeed, Owner.transform, Owner.CurrentMoveVector.Value);
            x = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, _moveSpeedX, Owner.HitInfo).x;
        }
        y = Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.jumpPower, Owner.ParamaterCon.GetParamater.fallSpeed).y;
        Owner.Rb.velocity = new Vector3(x, y);
    }
    

    public override void OnTranstion()
    {
        Owner.CurrentLocation
            .Skip(1)
            .Where(_ => IsActive)
            .Subscribe(currentLocation =>
            {
                _hitWall = Owner.HitInfo;
                switch (currentLocation)
                {
                    case StateOfPlayer.GripingWallEdge:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.GripingWallEdge);
                        break;
                    case StateOfPlayer.HangingWallEgde:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.HangingWallEgde);
                        break;
                    default:
                        Owner.PlayerStateMachine.Dispatch((int)currentLocation);
                        break;
                }
            }).AddTo(Owner);
    }
}

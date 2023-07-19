//日本語コメント可
using State = StateMachine<InputToPlayerMove>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorInAir : State
{
    float _moveSpeedX = 0f;

    public float MoveSpeedX { get => _moveSpeedX; set => _moveSpeedX = value; } 

    protected override void OnEnter(State prevState)
    {
       base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.Rb.velocity = new Vector3(Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, _moveSpeedX, Owner.HitInfo, Owner.AbleMove).x 
            ,Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.jumpPower,Owner.PlayerParamater.fallSpeed).y);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        //_beforeMoveVecX = 0f;
    }

    public override void OnTranstion()
    {
        Owner.CurrentLocation
            .Subscribe(currentLocation =>
            {
                switch (currentLocation)
                {
                    case StateOfPlayer.OnGround:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.OnGround);
                        break;
                    case StateOfPlayer.GripingWall:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.GripingWall);
                        break;
                    default:
                        break;
                }
            }).AddTo(Owner);
    }
}

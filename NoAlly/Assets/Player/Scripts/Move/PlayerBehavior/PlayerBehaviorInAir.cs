//日本語コメント可
using ActorBehaviourMove;
using State = StateMachine<PlayerMoveInput>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorInAir : State
{
    Vector3 _beforeMoveVec = Vector3.zero;

    public Vector3 BeforeMoveVec { get => _beforeMoveVec; set => _beforeMoveVec = value; } 

    protected override void OnEnter(State prevState)
    {
       base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        //Owner.Rb.velocity = _beforeMoveVec+ ActorMove.ActorVectorInAir(Owner.IsJump.Value,Owner.PlayerParamater.jumpPower);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
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

//日本語コメント可
using State = StateMachine<PlayerMoveInput>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorInAir : State
{
    float _beforeMoveVecX = 0f;

    public float BeforeMoveVecX { get => _beforeMoveVecX; set => _beforeMoveVecX = value; } 

    protected override void OnEnter(State prevState)
    {
       base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.Rb.velocity = new Vector3(_beforeMoveVecX ,Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.jumpPower).y);
        Debug.Log(Owner.Rb.velocity);
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

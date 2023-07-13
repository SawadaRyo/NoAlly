//日本語コメント可
using ActorBehaviourMove;
using UniRx;
using State = StateMachine<PlayerMoveInput>.State;

public class PlayerBehaviourOnWall : State
{
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
                    case StateOfPlayer.InAir:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
                        break;
                    default:
                        break;
                }
            }).AddTo(Owner);
    }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        ActorMove.ActorBehaviourOnWall(Owner.PlayerParamater.wallSlideSpeed
            ,Owner.Rb
            ,Owner.HitInfo
            ,Owner.CurrentLocation.Value);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }
}

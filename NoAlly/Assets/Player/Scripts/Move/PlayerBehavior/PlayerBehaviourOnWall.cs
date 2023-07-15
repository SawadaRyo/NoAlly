//日本語コメント可
using UniRx;
using UnityEngine;
using State = StateMachine<PlayerMoveInput>.State;

public class PlayerBehaviourOnWall : State
{
    public override void OnTranstion()
    {
        Owner.CurrentLocation
            .Where(_ => IsActive)
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
        Owner.IsJump
            .Where(_ => IsActive 
                     && Owner.IsJump.Value
                     && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isJump =>
            {
                Debug.Log(Owner.HitInfo.normal);
                Owner.Rb.velocity = new Vector3(Owner.HitInfo.normal.x * Owner.PlayerParamater.speed,Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.speed).y);
            });
    }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.WallBehaviour.ActorBehaviourOnWall(Owner.PlayerParamater.wallSlideSpeed
            ,Owner.Rb
            ,Owner.HitInfo
            ,Owner.CurrentLocation.Value);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }
}

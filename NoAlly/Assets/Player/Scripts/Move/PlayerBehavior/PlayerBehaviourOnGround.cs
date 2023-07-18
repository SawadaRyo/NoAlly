//日本語コメント可
using State = StateMachine<InputToPlayerMove>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviourOnGround : State
{

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.CurrentMoveVector.Value == Vector2.zero)
        {
            Owner.Rb.velocity = Vector3.zero;
        }
        else if (Owner.AbleMove && Owner.CurrentMoveVector.Value != Vector2.zero)
        {
            Owner.Rb.velocity = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.HitInfo);
            if (Mathf.Abs(Owner.GroundNormal.y) > 0.01f)
            {
                Owner.Rb.velocity = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.GroundNormal);
            }
        }
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        if (nextState is PlayerBehaviorInAir air)
        {
            air.MoveSpeedX = Owner.PlayerParamater.speed;
        }
        else if (nextState is PlayerBehaviourOnWall wall)
        {
            wall.MoveSpeedX = Owner.PlayerParamater.speed;
        }
    }

    public override void OnTranstion()
    {
        Debug.Log(Owner);
        Owner.CurrentLocation
            .Subscribe(currentLocation =>
            {
                switch (currentLocation)
                {
                    case StateOfPlayer.InAir:
                        Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
                        break;
                    default:
                        break;
                }
            }).AddTo(Owner);
        Owner.IsDash
            .Where(_ => IsActive
                     && Owner.IsDash.Value
                     && Owner.AbleDash)
            .Subscribe(isDash =>
            {
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.Dash);
            });
        Owner.IsJump
            .Where(_ => IsActive
                     && Owner.IsJump.Value
                     && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isjump =>
            {
                Owner.Rb.velocity = new Vector3(Owner.CurrentMoveVector.Value.x, Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.jumpPower, Owner.PlayerParamater.fallSpeed).y);
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
            });
    }
}

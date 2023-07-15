//日本語コメント可
using State = StateMachine<PlayerMoveInput>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviourOnGround : State
{
    float _veloX = 0f;


    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        _veloX = 0f;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.CurrentMoveVector.Value == Vector2.zero)
        {
            Owner.Rb.velocity = Vector3.zero;
        }
        else if (Owner.CurrentMoveVector.Value != Vector2.zero)
        {
            Owner.Rb.velocity = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.Rb, Owner.HitInfo.normal);
            if (Mathf.Abs(Owner.GroundNormal.y) > 0.01f)
            {
                Owner.Rb.velocity = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, Owner.PlayerParamater.speed, Owner.Rb, Owner.GroundNormal);
            }
        }
        _veloX = Owner.CurrentMoveVector.Value.x * Owner.PlayerParamater.speed;
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        if (nextState is PlayerBehaviorInAir air)
        {
            air.BeforeMoveVecX = _veloX;
            Debug.Log(_veloX);
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
            .Where(_ => Owner.IsDash.Value
                     && Owner.AbleDash)
            .Subscribe(isDash =>
            {
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.Dash);
            });
        Owner.IsJump
            .Where(_ => Owner.IsJump.Value && IsActive && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isjump =>
            {
                Owner.Rb.velocity =  new Vector3(_veloX, Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.jumpPower).y);
                Owner.PlayerStateMachine.Dispatch((int)StateOfPlayer.InAir);
            });
    }
}

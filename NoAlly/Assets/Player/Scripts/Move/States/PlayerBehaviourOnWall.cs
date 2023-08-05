//日本語コメント可
using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using State = StateMachine<PlayerBehaviorController>.State;

public class PlayerBehaviourOnWall : State
{
    float _moveSpeedX = 0f;
    RaycastHit _wallState;

    public RaycastHit HitWall => _wallState;

    public override void OnTranstion()
    {
        Owner.CurrentLocation
            .Where(_ => IsActive)
            .Subscribe(currentLocation =>
            {
                _wallState = Owner.HitInfo;
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
        Owner.IsJump
            .Where(_ => IsActive
                     && Owner.AbleWallJump
                     && Owner.IsJump.Value
                     && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isJump =>
            {
                Debug.Log(Owner.HitInfo.normal);
                float x = 0f;
                float y = 0f;
                if (Owner.IsDash.Value)
                {
                    x = Owner.HitInfo.normal.x * Owner.ParamaterCon.GetParamater.speed;
                }
                else
                {
                    x = Owner.HitInfo.normal.x * Owner.ParamaterCon.GetParamater.dashSpeed;
                }
                y = Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.speed, Owner.ParamaterCon.GetParamater.fallSpeed).y;
                Owner.MoveBehaviour.ActorRotateMethod(Owner.ParamaterCon.GetParamater.turnSpeed, Owner.transform, Owner.HitInfo.normal);
                Owner.Rb.velocity = new Vector3(x, y);
                AbleWallKick(Owner.ParamaterCon.GetParamater.wallKickInterval);
            });
    }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.AbleWallJump = true;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.WallBehaviour.ActorSlideWall(Owner.ParamaterCon.GetParamater.wallSlideSpeed
            , Owner.Rb
            , Owner.HitInfo
            , Owner.CurrentLocation.Value);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }

    /// <summary>
    /// 壁キックのインターバル
    /// </summary>
    /// <param name="interval"></param>
    async void AbleWallKick(float interval = 0.5f)
    {
        Owner.AbleMove = false;
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        Owner.AbleMove = true;
    }
}

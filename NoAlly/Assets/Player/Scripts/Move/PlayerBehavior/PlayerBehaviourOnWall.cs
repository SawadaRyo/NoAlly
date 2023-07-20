//日本語コメント可
using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using State = StateMachine<InputToPlayerMove>.State;

public class PlayerBehaviourOnWall : State
{
    float _moveSpeedX = 0f;
    RaycastHit _wallState;

    public RaycastHit WallState => _wallState;

    public float MoveSpeedX { get => _moveSpeedX; set => _moveSpeedX = value; }

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
                     && Owner.IsJump.Value
                     && !Owner.JumpBehaviour.KeyLook)
            .Subscribe(isJump =>
            {
                //Debug.Log(Owner.HitInfo.normal);
                Owner.Rb.velocity = new Vector3(Owner.HitInfo.normal.x * MoveSpeedX,Owner.JumpBehaviour.ActorVectorInAir(Owner.PlayerParamater.speed,Owner.PlayerParamater.fallSpeed).y);
                AbleWallKick(Owner.PlayerParamater.wallKickInterval);
            });
    }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        Owner.WallBehaviour.ActorSlideWall(Owner.PlayerParamater.wallSlideSpeed
            ,Owner.Rb
            ,Owner.HitInfo
            ,Owner.CurrentLocation.Value);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }

    /// <summary>
    /// 壁キックのインターバル
    /// </summary>
    /// <param name="interval"></param>
    async void AbleWallKick(float interval = 0.2f)
    {
        Owner.AbleMove = false;
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        Owner.AbleMove = true;
    }
}

//日本語コメント可
using System;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using State = StateMachine<PlayerBehaviorController>.State;

public class PlayerBehaviourOnWall : State
{
    float _moveSpeedX = 0f;
    float _moveVecX = 0f;
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
                        Owner.StateMachinePlayerMove.Dispatch((int)StateOfPlayer.GripingWallEdge);
                        break;
                    case StateOfPlayer.HangingWallEgde:
                        Owner.StateMachinePlayerMove.Dispatch((int)StateOfPlayer.HangingWallEgde);
                        break;
                    default:
                        Owner.StateMachinePlayerMove.Dispatch((int)currentLocation);
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
                _moveVecX = Owner.HitInfo.normal.x;
                _moveSpeedX = x;//x軸方向のベクトルを保存
                y = Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.speed, Owner.ParamaterCon.GetParamater.fallSpeed).y;
                Owner.MoveBehaviour.ActorRotateMethod(Owner.ParamaterCon.GetParamater.turnSpeed, Owner.transform, Owner.HitInfo.normal);
                Owner.Rb.velocity = new Vector3(x, y);
                Debug.Log(Owner.Rb.velocity);
                AbleWallKick(Owner.ParamaterCon.GetParamater.wallKickInterval);//壁キック中の入力制限
                Owner.AbleWallJump = false;
                Owner.StateMachinePlayerMove.Dispatch((int)StateOfPlayer.InAir);
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
        if(!Owner.AbleWallJump && nextState is PlayerBehaviorInAir inAir)
        {
            inAir.MoveSpeedX = _moveSpeedX;
            inAir.MoveVecX = _moveVecX;
        }
        _moveSpeedX = 0f;
        _moveVecX = 0f;
        Owner.AbleWallJump = false;
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

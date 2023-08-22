//日本語コメント可
using State = StateMachine<PlayerBehaviorController>.State;
using UnityEngine;
using UniRx;

public class PlayerBehaviorInAir : State
{
    float _moveSpeedX = 0f;
    float _moveVecX = 0f;
    RaycastHit _hitWall;

    public RaycastHit HitWall => _hitWall;
    public float MoveSpeedX { get => _moveSpeedX; set => _moveSpeedX = value; }
    public float MoveVecX { get => _moveVecX; set => _moveVecX = value; }

    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        if (prevState is PlayerBehaviourOnWall)
        {
            Debug.Log(_moveVecX);
        }
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        float x = 0f;
        float y = 0f;
        if (Owner.AbleMove)
        {
            Owner.MoveBehaviour.ActorRotateMethod(Owner.ParamaterCon.GetParamater.turnSpeed, Owner.transform, Owner.CurrentMoveVector.Value);
            x = Owner.MoveBehaviour.ActorMoveMethod(Owner.CurrentMoveVector.Value.x, _moveSpeedX, Owner.HitInfo).x;
        }
        else
        {
            //x = Owner.MoveBehaviour.ActorMoveMethod(_moveVecX, _moveSpeedX, Owner.HitInfo).x;
            x = _moveVecX * _moveSpeedX;
            if (Owner.HitInfo.collider != null)
            {
                Debug.Log(Owner.HitInfo.normal);
            }
            Debug.Log(x);
            //Debug.Log($"{x},{y}");
        }
        y = Owner.JumpBehaviour.ActorVectorInAir(Owner.ParamaterCon.GetParamater.jumpPower, Owner.ParamaterCon.GetParamater.fallSpeed).y;
        Owner.Rb.velocity = new Vector3(x, y);
    }
    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        //_moveSpeedX = 0f;
        //_moveVecX = 0f;
    }

    public override void OnTranstion()
    {
        Owner.CurrentLocation
            .Skip(1)
            .Where(_ => IsActive)
            .Subscribe(currentLocation =>
            {
                _hitWall = Owner.HitInfo;
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
    }
}

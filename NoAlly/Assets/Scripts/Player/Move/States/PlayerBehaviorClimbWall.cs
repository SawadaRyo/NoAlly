//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<PlayerBehaviorController>.State;

public class PlayerBehaviorClimbWall : State
{
    protected override void OnEnter(State prevState)
    {
        //if(prevState is PlayerBehaviourOnWall wall)
        //{
        //    Owner.WallBehaviour.ClimbWall(Owner.Rb,wall.HitWall);
        //}
        if(prevState is PlayerBehaviorInAir air)
        {
            Owner.WallBehaviour.ClimbWall(Owner.Rb, air.HitWall);
        }
    }
}

//日本語コメント可
using ActorBehaviour.Jump;
using ActorBehaviour.Move;
using ActorBehaviour.Wall;
using UnityEngine;
using UniRx;

public interface IHumanoid
{
    public ActorAir JumpBehaviour { get; }
    public ActorMove MoveBehaviour { get; }
    public ActorWall WallBehaviour { get; }
}

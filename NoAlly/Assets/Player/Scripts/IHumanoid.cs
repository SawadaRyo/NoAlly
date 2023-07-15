//日本語コメント可
using ActorBehaviour.Jump;
using ActorBehaviour.Move;
using ActorBehaviour.Wall;
using UnityEngine;
using UniRx;

public interface IHumanoid
{
    public bool AbleDash { get; }
    public ActorParamater PlayerParamater { get; }
    public ActorAir JumpBehaviour { get; }
    public ActorMove MoveBehaviour { get; }
    public ActorWall WallBehaviour { get; }
    public IReadOnlyReactiveProperty<bool> IsDash { get; }
    public IReadOnlyReactiveProperty<bool> IsJump { get; }
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector { get; }
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation { get; }
}

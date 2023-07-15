//日本語コメント可
using UniRx;
using UnityEngine;

public interface IActor<T>
{
    public StateMachine<T> PlayerStateMachine { get; }
    public Rigidbody Rb { get; }
    public Vector3 GroundNormal { get; }
    public RaycastHit HitInfo { get; }
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector { get; }
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation { get; }
}

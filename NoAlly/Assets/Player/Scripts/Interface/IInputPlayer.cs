//日本語コメント可
using UnityEngine;
using UniRx;

public interface IInputPlayer
{
    public bool AbleDash { get; }
    public IReadOnlyReactiveProperty<bool> IsDash { get; }
    public IReadOnlyReactiveProperty<bool> IsJump { get; }
    public IReadOnlyReactiveProperty<Vector2> CurrentMoveVector { get; }
    public IReadOnlyReactiveProperty<StateOfPlayer> CurrentLocation { get; }
}

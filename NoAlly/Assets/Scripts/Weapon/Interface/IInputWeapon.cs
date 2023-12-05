//日本語コメント可
using UniRx;

public interface IInputWeapon
{
    public IReadOnlyReactiveProperty<bool> IsSwichWeapon { get; }
    public IReadOnlyReactiveProperty<bool> InputAttackDown { get; }
    public IReadOnlyReactiveProperty<bool> InputAttackCharge { get; }
    public IReadOnlyReactiveProperty<bool> InputAttackUp { get; }
}

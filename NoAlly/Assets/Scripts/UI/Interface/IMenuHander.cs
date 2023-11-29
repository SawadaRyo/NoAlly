using UnityEngine;
using UniRx;

public interface IMenuHander
{
    public IReadOnlyReactiveProperty<(int, int)> InputCross { get; }
    public IReadOnlyReactiveProperty<bool> IsDiside { get; }
    public void Initialize();
}

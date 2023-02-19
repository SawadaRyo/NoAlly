using UniRx;

public interface IMenuHander<T> where T : ICommandButton
{
    IReadOnlyReactiveProperty<int> CrossH { get; }
    IReadOnlyReactiveProperty<int> CrossV { get; }
    IReadOnlyReactiveProperty<bool> IsDiside { get; }
    T SelectButton { get; set; }
    public void Initialize(T[] allbuttons);
    public void SelectCommand(float h, float v);
}

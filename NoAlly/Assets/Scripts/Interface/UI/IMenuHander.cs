

public interface IMenuHander<T> where T:ICommandButton
{
    public void Initialize(T[] allbuttons);
    public T SelectButton(float h, float v);
    public void DisideCommand(T targetButton);
}

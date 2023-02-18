using UnityEngine.UI;

public interface ICommandButton
{
    public Button Command { get; }
    public ButtonState State { get; }
    public CommandType TypeOfCommand { get; }
    public void Selected(bool isSelect);
    public void Disaide(bool isDisaide);

}
public enum ButtonState : int
{
    None,
    Selected,
    Disided
}
public enum CommandType
{
    MAIN = 0,
    SUB = 1,
    ElEMENT = 2
}


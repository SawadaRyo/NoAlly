using UnityEngine.UI;

public interface ICommandButton
{
    /// <summary>
    /// オブジェクトのボタン
    /// </summary>
    public Button Command { get; }
    /// <summary>
    /// ボタンの状態
    /// </summary>
    public ButtonState State { get; }
    /// <summary>
    /// ボタンのコマンドタイプ
    /// </summary>
    public CommandType TypeOfCommand { get; }
    /// <summary>
    /// 選択中
    /// </summary>
    /// <param name="isSelect"></param>
    public void Selected(bool isSelect);
    /// <summary>
    /// 決定
    /// </summary>
    /// <param name="isDisaide"></param>
    public void Disaide(bool isDisaide);

}
public enum ButtonState : int
{
    NONE, //通常
    SELECTED, //選択中
    DISIDED //決定済み
}
public enum CommandType
{
    NONE = -1,
    MAIN = 0, //メイン武器
    SUB = 1, //サブ武器
    ELEMENT = 2 //属性
}


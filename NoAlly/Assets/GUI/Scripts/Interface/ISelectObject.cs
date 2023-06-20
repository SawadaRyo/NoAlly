using Cysharp.Threading.Tasks;

public interface ISelectObject
{
    /// <summary>
    /// このオブジェクトの親関係
    /// </summary>
    public SelectObjecArrayBase Perent { get; }
    /// <summary>
    /// ゲーム実行時初期化
    /// </summary>
    public void Initialize(SelectObjecArrayBase perent);

    /// <summary>
    /// 選択時実行関数
    /// </summary>
    /// <param name="isSelect"></param>
    public void IsSelect(bool isSelect);
    /// <summary>
    /// ボタンのイベントを実行する
    /// </summary>
    public void DoEvent(bool isDoEvent);
    /// <summary>
    /// メニュー展開時実行関数
    /// </summary>
    public void MenuExtended();
    /// <summary>
    /// メニュー収納実行関数
    /// </summary>
    public void MenuClosed();
}

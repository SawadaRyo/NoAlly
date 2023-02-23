

public interface IObjectPool<TOwner> where TOwner : IObjectGenerator
{
    /// <summary>
    /// オブジェクトが起動中かどうか
    /// </summary>
    public bool IsActive { get; }
    public TOwner Generator { get; }
    /// <summary>
    /// オブジェクトが有効になった時に呼ばれる関数
    /// </summary>
    public void Create();
    /// <summary>
    /// オブジェクトが非有効になった時に呼ばれる関数
    /// </summary>
    public void Disactive();
    /// <summary>
    /// オブジェクトが非有効になった時に呼ばれる関数,(時間制限付き)
    /// </summary>
    public void Disactive(float interval);
    /// <summary>
    /// オブジェクトが生成された時に呼ばれる関数
    /// </summary>
    /// <typeparam name="TOwner">このオブジェクトを使用するオーナーのジェネリッククラス</typeparam>
    /// <param name="Owner">このオブジェクトを使用するオーナー</param>
    public void DisactiveForInstantiate(TOwner Owner);
}

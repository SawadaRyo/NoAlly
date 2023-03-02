

public interface ISoundObjectPool<TOwner> where TOwner : IObjectGenerator
{
    /// <summary>
    /// オブジェクトが起動中かどうか
    /// </summary>
    public bool IsActive { get; }
    public TOwner Owner { get; }
    /// <summary>
    /// オブジェクトが有効になった時に呼ばれる関数
    /// </summary>
    public void Create(SoundType type,int soundNumber);
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
    /// <param name="owner">このオブジェクトを使用するオーナー</param>
    public void DisactiveForInstantiate(TOwner owner);
}


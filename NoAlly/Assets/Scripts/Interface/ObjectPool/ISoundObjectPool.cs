

public interface ISoundObjectPool<TOwner> :IObjectPool<TOwner> where TOwner : IObjectGenerator
{
    /// <summary>
    /// オブジェクトが有効になった時に呼ばれる関数
    /// </summary>
    public void Create(SoundType type,int soundNumber);
}


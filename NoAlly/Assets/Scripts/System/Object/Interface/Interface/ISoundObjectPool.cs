

public interface ISoundObjectPool<TOwner> :IObjectPool<TOwner> where TOwner : IObjectGenerator
{
    /// <summary>
    /// �I�u�W�F�N�g���L���ɂȂ������ɌĂ΂��֐�
    /// </summary>
    public void Create(SoundType type,int soundNumber);
}


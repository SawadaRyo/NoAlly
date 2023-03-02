

public interface ISoundObjectPool<TOwner> where TOwner : IObjectGenerator
{
    /// <summary>
    /// �I�u�W�F�N�g���N�������ǂ���
    /// </summary>
    public bool IsActive { get; }
    public TOwner Owner { get; }
    /// <summary>
    /// �I�u�W�F�N�g���L���ɂȂ������ɌĂ΂��֐�
    /// </summary>
    public void Create(SoundType type,int soundNumber);
    /// <summary>
    /// �I�u�W�F�N�g����L���ɂȂ������ɌĂ΂��֐�
    /// </summary>
    public void Disactive();
    /// <summary>
    /// �I�u�W�F�N�g����L���ɂȂ������ɌĂ΂��֐�,(���Ԑ����t��)
    /// </summary>
    public void Disactive(float interval);
    /// <summary>
    /// �I�u�W�F�N�g���������ꂽ���ɌĂ΂��֐�
    /// </summary>
    /// <typeparam name="TOwner">���̃I�u�W�F�N�g���g�p����I�[�i�[�̃W�F�l���b�N�N���X</typeparam>
    /// <param name="owner">���̃I�u�W�F�N�g���g�p����I�[�i�[</param>
    public void DisactiveForInstantiate(TOwner owner);
}


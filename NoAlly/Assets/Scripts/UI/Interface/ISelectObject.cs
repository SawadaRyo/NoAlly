using Cysharp.Threading.Tasks;

public interface ISelectObject
{
    /// <summary>
    /// ���̃I�u�W�F�N�g�̐e�֌W
    /// </summary>
    public SelectObjecArrayBase Perent { get; }
    /// <summary>
    /// �Q�[�����s��������
    /// </summary>
    public void SetUp(SelectObjecArrayBase perent);

    /// <summary>
    /// �I�������s�֐�
    /// </summary>
    /// <param name="isSelect"></param>
    public void IsSelect(bool isSelect);
    /// <summary>
    /// �{�^���̃C�x���g�����s����
    /// </summary>
    public void ClickEvent();
    public void Canseled();
    /// <summary>
    /// ���j���[�W�J�����s�֐�
    /// </summary>
    public void MenuExtended();
    /// <summary>
    /// ���j���[���[���s�֐�
    /// </summary>
    public void MenuClosed();
}
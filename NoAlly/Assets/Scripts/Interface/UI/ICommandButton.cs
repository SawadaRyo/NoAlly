using UnityEngine.UI;

public interface ICommandButton
{
    /// <summary>
    /// �I�u�W�F�N�g�̃{�^��
    /// </summary>
    public Button Command { get; }
    /// <summary>
    /// �{�^���̏��
    /// </summary>
    public ButtonState State { get; }
    /// <summary>
    /// �{�^���̃R�}���h�^�C�v
    /// </summary>
    public CommandType TypeOfCommand { get; }
    /// <summary>
    /// �I��
    /// </summary>
    /// <param name="isSelect"></param>
    public void Selected(bool isSelect);
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="isDisaide"></param>
    public void Disaide(bool isDisaide);

}
public enum ButtonState : int
{
    None, //�ʏ�
    Selected, //�I��
    Disided //����ς�
}
public enum CommandType
{
    MAIN = 0, //���C������
    SUB = 1, //�T�u����
    ELEMENT = 2 //����
}


using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CommandButton : MonoBehaviour, ICommandButton
{
    [SerializeField, Header("ボタン本体")]
    Button _button;
    [SerializeField, Header("ボタンのAnimator")]
    Animator _animator;
    [SerializeField, Header("ボタンの色")]
    Color[] _buttonColors = new Color[]
    #region
    {
        Color.white,
        Color.yellow
    };
    #endregion
    [SerializeField, Tooltip("")]
    CommandType _commandType;


    [Tooltip("")]
    ButtonState _state;

    public Button Command => _button;
    public ButtonState State => _state;
    public CommandType TypeOfCommand => _commandType;

    public void Selected(bool isSelect)
    {
        if (isSelect)
        {
            if (_state != ButtonState.Disided)
            {
                _state = ButtonState.Selected;
            }
        }
        else
        {
            switch (_state)
            {
                case ButtonState.Selected:
                    if (_button.image)
                    {
                        _button.image.color = _buttonColors[0];
                    }
                    _state = ButtonState.None;
                    break;
                case ButtonState.Disided:
                    if (_button.image)
                    {
                        _button.image.color = _buttonColors[1];
                    }
                    break;
                default:
                    break;
            }
        }
        _animator.SetBool("IsSelect", isSelect);

    }
    public void Disaide(bool isDisaide)
    {
        //_animator.SetBool("IsDisaide", isDisaide);
        if (isDisaide)
        {
            _button.onClick.Invoke();
            if (_button.image)
            {
                _button.image.color = _buttonColors[1];
            }
            _state = ButtonState.Disided;
        }
        else
        {
            if (_button.image)
            {
                _button.image.color = _buttonColors[0];
            }
            _state = ButtonState.None;
        }

    }
}


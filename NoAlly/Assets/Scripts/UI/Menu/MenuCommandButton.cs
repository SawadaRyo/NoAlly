using UnityEngine;
using UnityEngine.UI;

public class MenuCommandButton
{
    ButtonState _state;
    Animator _animator;
    Button _commund;
    WeaponType _weaponName;
    Color[] _buttonColors = new Color[]
    #region
    {
        Color.white,
        Color.yellow
    };
    #endregion
    ElementType _elementType;
    CommandType _type;

    public ButtonState State => _state;
    public Button Command => _commund;
    public WeaponType TypeOfWeapon => _weaponName;
    public ElementType TypeOfElement => _elementType;
    public CommandType TypeOfCommand => _type;

    public MenuCommandButton(ButtonState state, Button button)
    {
        _state = state;
        _commund = button;
        _animator = _commund.GetComponent<Animator>();
    }
    public MenuCommandButton(ButtonState state, Button button, WeaponType name, CommandType type)
    {
        _state = state;
        _commund = button;
        if (_commund != null)
        {
            _animator = _commund.GetComponent<Animator>();
        }
        _weaponName = name;
        _type = type;
    }
    public MenuCommandButton(ButtonState state, Button button, ElementType element, CommandType type)
    {
        _state = state;
        _commund = button;
        if (_commund != null)
        {
            _animator = _commund.GetComponent<Animator>();
        }
        _elementType = element;
        _type = type;
    }

    public void Selected(bool isSelect)
    {
        if (isSelect)
        {
            if (_state != ButtonState.Disided)
            {
                _state = ButtonState.Selected;
            }
            _animator.enabled = isSelect;
        }
        else
        {
            switch (_state)
            {
                case ButtonState.Selected:
                    if (_commund.image)
                    {
                        _commund.image.color = _buttonColors[0];
                    }
                    _state = ButtonState.None;
                    break;
                case ButtonState.Disided:
                    if (_commund.image)
                    {
                        _commund.image.color = _buttonColors[1];
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
            _commund.onClick.Invoke();
            if (_commund.image)
            {
                _commund.image.color = _buttonColors[1];
            }
            _state = ButtonState.Disided;
        }
        else
        {
            if (_commund.image)
            {
                _commund.image.color = _buttonColors[0];
            }
            _state = ButtonState.None;
        }

    }
    public enum ButtonState : int
    {
        None,
        Selected,
        Disided
    }
}


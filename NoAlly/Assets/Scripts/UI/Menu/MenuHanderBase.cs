using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuHanderBase : MonoBehaviour
{
    [SerializeField, Tooltip("�{�^���I���̃C���^�[�o��")]
    float _interval = 0.3f;


    [Tooltip("���j���[��ʂ̊J�m�F")]
    bool _menuIsOpen = true;
    [Tooltip("�c����")]
    int _crossV = 0;
    [Tooltip("")]
    Image[] _gameUIGauges = default;
    [Tooltip("")]
    Image[] _gamePanelsImages = default;
    [Tooltip("")]
    GameObject _canvas = default;
    [Tooltip("�I�𒆂̃{�^��")]
    MenuCommandButton _targetButton = default;
    [Tooltip("")]
    MenuCommandButton _selectedButtons = null;
    [Tooltip("")]
    MenuCommandButton[] _allButtons = null;
    [Tooltip("")]
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;

    

    public void Initialize()
    {
        _canMove = new Interval(_interval);

        //Canvas��̑S�Ă�Button���擾����B
        Button[] buttonArray = GetComponentsInChildren<Button>(true);
        _allButtons = new MenuCommandButton[buttonArray.Length];

        for(int i = 0;i < _allButtons.Length; i++)
        {
            _allButtons[i] = new MenuCommandButton(MenuCommandButton.ButtonState.None, buttonArray[i]);
        }

        //UI�̃Q�[���N�����̏����ݒ�
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _gamePanelsImages = _canvas.GetComponentsInChildren<Image>(true);
        foreach (Image gpi in _gamePanelsImages)
        {
            gpi.enabled = true;
        }
        _targetButton = _allButtons[0];
        _targetButton.Selected(true);
    }

    void Update()
    {
        //if (Input.GetButtonDown("MenuSwitch"))
        {
            //_menuIsOpen = !_menuIsOpen;
            //IsManuExtend(_menuIsOpen);
        }

        if (_menuIsOpen)
        {
            float v = Input.GetAxisRaw("CrossKeyV");

            if (_canMove.IsCountUp() && (v != 0))
            {
                MenuCommandButton b = SelectButton(v);
                if (b.Command)
                {
                    _targetButton.Selected(false);
                    _targetButton = b;
                    _targetButton.Selected(true);
                }
            }

            if (Input.GetButtonDown("Decision"))
            {
                DisideCommand(_targetButton);
            }
        }
    }

    /// <summary>
    /// ���j���[��ʂ̓W�J
    /// </summary>
    /// <param name="isOpen"></param>
    void IsManuExtend(bool isOpen)
    {
        //ToDo ���j���[�̊J�ɃA�j���[�V������������
        foreach (Image gpi in _gamePanelsImages)
        {
            //if (gpi.gameObject.tag == "ChackFrame") return;
            gpi.enabled = isOpen;
        }
        foreach (MenuCommandButton b in _allButtons)
        {
            b.Command.enabled = isOpen;
        }

        foreach (Image image in _gameUIGauges)
        {
            image.enabled = !isOpen;
        }
    }
    /// <summary>
    /// ���j���[�̃{�^������
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    MenuCommandButton SelectButton(float v)
    {
        _canMove.ResetTimer();
        if (v < 0)
        {
            _crossV++;
            if (_crossV > _allButtons.GetLength(0) - 1) _crossV = 0;
        }
        if (v > 0)
        {
            _crossV--;
            if (_crossV < 0) _crossV = _allButtons.GetLength(0) - 1;
        }
        return _allButtons[_crossV];
    }
    /// <summary>
    /// �I�����ꂽ�{�^���ɓo�^���ꂽ�֐������s����֐�
    /// </summary>
    /// <param name="targetButton"></param>
    void DisideCommand(MenuCommandButton targetButton)
    {
        _targetButton.Disaide(true);
    }
}

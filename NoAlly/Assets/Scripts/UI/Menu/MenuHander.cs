using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuHander : SingletonBehaviour<MenuHander>
{
    [SerializeField, Tooltip("�{�^���I���̃C���^�[�o��")]
    float _interval = 0.3f;
    [SerializeField, Tooltip("���C�����j���[�̃v���n�u")] WeaponEquipment _mainManu = null;


    [Tooltip("���j���[��ʂ̊J�m�F")] 
    bool _menuIsOpen = false;
    [Tooltip("������")] 
    int _crossH = 0;
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
    MenuCommandButton[] _selectedButtons = null;
    [Tooltip("")]
    MenuCommandButton[,] _allButtons = default;
    [Tooltip("")] 
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;

    void Start()
    {
        Initializer();
    }

    public void Initializer()
    {
        _canMove = new Interval(_interval);

        _allButtons = new MenuCommandButton[Enum.GetNames(typeof(CommandType)).Length, Enum.GetNames(typeof(ElementType)).Length]; 
        _selectedButtons = new MenuCommandButton[Enum.GetNames(typeof(CommandType)).Length];
        int length = 0;

        //Canvas��̑S�Ă�Button���擾����B
        Button[] buttonArray = GetComponentsInChildren<Button>(true);

        //MenuCommandButton���C���X�^���X����
        for (int y = 0; y < _allButtons.GetLength(0); y++)
        {
            for (int x = 0; x < _allButtons.GetLength(1); x++)
            {
                if ((CommandType)y != CommandType.ElEMENT)
                {
                    _allButtons[y, x] = new MenuCommandButton(MenuCommandButton.ButtonState.None, buttonArray[length], (WeaponType)x, (CommandType)y);
                    _selectedButtons[y] = new MenuCommandButton(MenuCommandButton.ButtonState.None, null, (WeaponType)x, (CommandType)y);
                }
                else
                {
                    _allButtons[y, x] = new MenuCommandButton(MenuCommandButton.ButtonState.None, buttonArray[length], (ElementType)x, (CommandType)y);
                    _selectedButtons[y] = new MenuCommandButton(MenuCommandButton.ButtonState.None, null, (ElementType)x, (CommandType)y);
                }
                _allButtons[y, x].Command.enabled = false;
                length++;
            }
        }
        _selectedButtons[(int)CommandType.MAIN] = _allButtons[(int)CommandType.MAIN, (int)_mainManu.MainWeapon.Value.Type];
        _selectedButtons[(int)CommandType.MAIN].Disaide(true);
        _selectedButtons[(int)CommandType.SUB] = _allButtons[(int)CommandType.SUB, (int)_mainManu.SubWeapon.Value.Type];
        _selectedButtons[(int)CommandType.SUB].Disaide(true);
        _selectedButtons[(int)CommandType.ElEMENT] = _allButtons[(int)CommandType.ElEMENT, (int)ElementType.RIGIT];
        _selectedButtons[(int)CommandType.ElEMENT].Disaide(true);

        //UI�̃Q�[���N�����̏����ݒ�
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _gamePanelsImages = _canvas.GetComponentsInChildren<Image>(true);
        foreach (Image gpi in _gamePanelsImages)
        {
            gpi.enabled = false;
        }

        _gameUIGauges = _canvas.transform.GetChild(2).GetComponentsInChildren<Image>();
        foreach (Image image in _gameUIGauges)
        {
            image.enabled = true;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("MenuSwitch"))
        {
            _menuIsOpen = !_menuIsOpen;
            IsManuExpand(_menuIsOpen);
        }

        if (_menuIsOpen)
        {
            float h = Input.GetAxisRaw("CrossKeyH");
            float v = Input.GetAxisRaw("CrossKeyV");

            if (_canMove.IsCountUp() && (h != 0 || v != 0))
            {
                MenuCommandButton b = SelectButton(h, v);
                if (b.Command)
                {
                    _targetButton.Selected(false);
                    _targetButton = b;
                    _targetButton.Selected(true);
                    //_targetButton.Command.Select();
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
    void IsManuExpand(bool isOpen)
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

        if (isOpen)
        {
            MenuOpen();
        }
        else
        {
            MenuClose();
        }
    }
    /// <summary>
    /// ���j���[�̃{�^������
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    MenuCommandButton SelectButton(float h, float v)
    {
        _canMove.ResetTimer();
        if (h > 0)
        {
            _crossH++;
            if (_crossH > _allButtons.GetLength(1) - 1) _crossH = 0;
        }
        if (v < 0)
        {
            _crossV++;
            if (_crossV > _allButtons.GetLength(0) - 1) _crossV = 0;
        }
        if (h < 0)
        {
            _crossH--;
            if (_crossH < 0) _crossH = _allButtons.GetLength(1) - 1;
        }
        if (v > 0)
        {
            _crossV--;
            if (_crossV < 0) _crossV = _allButtons.GetLength(0) - 1;
        }
        return _allButtons[_crossV, _crossH];
    }
    /// <summary>
    /// �I�����ꂽ�{�^���ɓo�^���ꂽ�֐������s����֐�
    /// </summary>
    /// <param name="targetButton"></param>
    void DisideCommand(MenuCommandButton targetButton)
    {
        if (_selectedButtons[(int)targetButton.TypeOfCommand].Command != null)
        {
            _selectedButtons[(int)targetButton.TypeOfCommand].Command.image.color = Color.white;
        }

        switch (targetButton.TypeOfCommand)
        {
            case CommandType.MAIN:
                if (targetButton.TypeOfWeapon == _selectedButtons[(int)CommandType.SUB].TypeOfWeapon)
                {
                    var beforeMainWeapon = _selectedButtons[(int)CommandType.MAIN];
                    _selectedButtons[(int)CommandType.SUB].Disaide(false);
                    _selectedButtons[(int)CommandType.SUB] = _allButtons[(int)CommandType.SUB, (int)beforeMainWeapon.TypeOfWeapon];
                    _selectedButtons[(int)CommandType.SUB].Disaide(true);
                }
                break;
            case CommandType.SUB:
                if (targetButton.TypeOfWeapon == _selectedButtons[(int)CommandType.MAIN].TypeOfWeapon)
                {
                    var beforeSubWeapon = _selectedButtons[(int)CommandType.SUB];
                    _selectedButtons[(int)CommandType.MAIN].Disaide(false);
                    _selectedButtons[(int)CommandType.MAIN] = _allButtons[(int)CommandType.MAIN, (int)beforeSubWeapon.TypeOfWeapon];
                    _selectedButtons[(int)CommandType.MAIN].Disaide(true);
                }
                break;
            default:
                break;
        }

        targetButton.Disaide(true);
        _selectedButtons[(int)targetButton.TypeOfCommand] = targetButton;
    }
    /// <summary>
    /// ���j���[��ʓW�J���ɌĂԊ֐�
    /// </summary>
    void MenuOpen()
    {
        _allButtons[_crossV, _crossH].Command.Select();
        _targetButton = _allButtons[_crossV, _crossH];
    }
    /// <summary>
    /// ���j���[��ʏk�����ɌĂԊ֐�
    /// </summary>
    void MenuClose()
    {

    }
}
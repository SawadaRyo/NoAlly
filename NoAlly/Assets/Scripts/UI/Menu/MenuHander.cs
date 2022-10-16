using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuHander : SingletonBehaviour<MenuHander>
{
    [SerializeField] float _interval = 0.2f;


    bool _menuIsOpen = false;
    int _crossH = 0;
    int _crossV = 0;
    Image[] _gameUIGauges = default;
    Image[] _gamePanelsImages = default;
    GameObject _canvas = default;
    MenuCommandButton _targetButton = default;
    MenuCommandButton[] _selectedButtons = null;
    MenuCommandButton[,] _allButtons = default;
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;

    public void Init()
    {
        _canMove = new Interval(_interval);

        //Canvas上の全てのButtonを取得する。
        _allButtons = new MenuCommandButton[Enum.GetNames(typeof(CommandType)).Length, Enum.GetNames(typeof(ElementType)).Length];
        _selectedButtons = new MenuCommandButton[Enum.GetNames(typeof(CommandType)).Length];
        int length = 0;
        Button[] buttonArray = GetComponentsInChildren<Button>(true);

        for (int y = 0; y < _allButtons.GetLength(0); y++)
        {
            for (int x = 0; x < _allButtons.GetLength(1); x++)
            {
                if ((CommandType)y != CommandType.ElEMENT)
                {
                    _allButtons[y, x] = new MenuCommandButton(false, buttonArray[length], (WeaponName)x, (CommandType)y);
                    _selectedButtons[y] = new MenuCommandButton(false, null, (WeaponName)x, (CommandType)y);
                }
                else
                {
                    _allButtons[y, x] = new MenuCommandButton(false, buttonArray[length], (ElementType)x, (CommandType)y);
                    _selectedButtons[y] = new MenuCommandButton(false, null, (ElementType)x, (CommandType)y);
                }
                _allButtons[y, x].Command.enabled = false;
                length++;
            }
            //DisideCommand(_selectedButtons);
        }

        //UIのゲーム起動時の初期設定
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
                    _targetButton = b;
                    _targetButton.Command.Select();
                }
            }

            if (Input.GetButtonDown("Decision"))
            {
                DisideCommand(_targetButton);
                _targetButton.Command.onClick.Invoke();
            }
        }
    }

    /// <summary>メニュー画面の展開</summary>
    /// <param name="isOpen"></param>
    void IsManuExpand(bool isOpen)
    {
        //ToDo メニューの開閉にアニメーションを加える
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
    /// <summary>メニューのボタン操作</summary>
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
    void DisideCommand(MenuCommandButton selectedButton)
    {

        switch (selectedButton.Type)
        {
            case CommandType.MAIN:
                if (selectedButton.Name == _selectedButtons[(int)CommandType.SUB].Name)
                {
                    _selectedButtons[(int)CommandType.SUB] = _allButtons[(int)CommandType.SUB, (int)selectedButton.Name];
                    _selectedButtons[(int)CommandType.SUB].Command.image.color = Color.yellow;
                }
                break;
            case CommandType.SUB:
                if (selectedButton.Name == _selectedButtons[(int)CommandType.MAIN].Name)
                {
                    _selectedButtons[(int)CommandType.MAIN] = _allButtons[(int)CommandType.MAIN, (int)selectedButton.Name];
                    _selectedButtons[(int)CommandType.MAIN].Command.image.color = Color.yellow;
                }
                break;
            default:
                break;
        }

        if (_selectedButtons[(int)selectedButton.Type].Command != null)
        {
            _selectedButtons[(int)selectedButton.Type].Command.image.color = Color.white;
        }
        _selectedButtons[(int)selectedButton.Type] = selectedButton;
        _selectedButtons[(int)selectedButton.Type].Command.image.color = Color.yellow;
    }
    /// <summary>メニュー画面展開時に呼ぶ関数 </summary>
    void MenuOpen()
    {
        _allButtons[_crossV, _crossH].Command.Select();
        _targetButton = _allButtons[_crossV, _crossH];
    }
    /// <summary>メニュー画面縮小時に呼ぶ関数</summary>
    void MenuClose()
    {

    }
}
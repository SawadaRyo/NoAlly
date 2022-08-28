using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuHander : SingletonBehaviour<MenuHander>
{
    [SerializeField] float _interval = 0.2f;


    bool _menuIsOpen = false;
    bool _safeZorn = false;
    int _crossH = 0;
    int _crossV = 0;
    Image[] _gameUIGauges = default;
    Image[] _gamePanelsImages = default;
    GameObject _canvas = default;
    Button _targetButton = default;
    Button[,] _allButtons = default;
    Interval _canMove = default;
    public bool SafeZorn { get => _safeZorn; set => _safeZorn = value; }
    public bool MenuIsOpen => _menuIsOpen;

    void Start()
    {
        _canMove = new Interval(_interval);

        //Canvas上の全てのButtonを取得する。
        _allButtons = new Button[Enum.GetNames(typeof(EquipmentType)).Length, Enum.GetNames(typeof(ElementType)).Length];
        int length = 0;
        Button[] buttonArray = GetComponentsInChildren<Button>(true);

        for (int y = 0; y < _allButtons.GetLength(0); y++)
        {
            for (int x = 0; x < _allButtons.GetLength(1); x++)
            {
                _allButtons[y, x] = buttonArray[length];
                _allButtons[y, x].enabled = false;
                length++;
            }
        }

        //ToDo マジックナンバーを直す。
        //for (int i = 0; i < _gamePanels.Length; i++)
        //{
        //    _gamePanels[i] = _canvas.transform.GetChild(i).gameObject;
        //    _gamePanels[i].SetActive(false);
        //}

        //UIのゲーム起動時の初期設定
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _gamePanelsImages = GetComponentsInChildren<Image>(true);
        foreach(Image gpi in _gamePanelsImages)
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

            Debug.Log(_canMove.IsCountUp());
            if (_canMove.IsCountUp() && (h != 0 || v != 0))
            {
                //if (_targetButton != null)
                Button b = SelectButton(h, v);
                if (b != null)
                {
                    _targetButton = b;
                    _targetButton.Select();
                }
            }

            if (Input.GetButtonDown("Decision"))
            {
                _targetButton.onClick.Invoke();
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
        foreach(Button b in _allButtons)
        {
            b.enabled = isOpen;
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

    Button SelectButton(float h, float v)
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

    /// <summary>メニュー画面展開時に呼ぶ関数 </summary>
    void MenuOpen()
    {
        _allButtons[_crossV, _crossH].Select();
        _targetButton = _allButtons[_crossV, _crossH];
    }

    /// <summary>メニュー画面縮小時に呼ぶ関数</summary>
    void MenuClose()
    {

    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuHander : SingletonBehaviour<MenuHander>
{
    [SerializeField] float _interval = 1f;

    Image[] _gameUIGauges = default;
    GameObject[] _gamePanels = new GameObject[2];

    bool _menuIsOpen = false;
    bool _safeZorn = false;
    float _crossH = 0;
    float _crossV = 0;
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
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _allButtons = new Button[Enum.GetNames(typeof(EquipmentType)).Length, Enum.GetNames(typeof(ElementType)).Length];
        int length = 0;
        Button[] buttonArray = GetComponentsInChildren<Button>(true);

        for (int y = 0; y < _allButtons.GetLength(0); y++)
        {
            for (int x = 0; x < _allButtons.GetLength(1); x++)
            {
                _allButtons[y, x] = buttonArray[length];
                length++;
            }
        }

        //UIのゲーム起動時の初期設定
        //ToDo マジックナンバーを直す。
        for (int i = 0; i < 2; i++)
        {
            _gamePanels[i] = _canvas.transform.GetChild(i).gameObject;
            _gamePanels[i].SetActive(false);
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
            if (_canMove.IsCountUp())
            {
                _targetButton = SelectButton(h, v);
                if(_targetButton != null)
                {
                    _targetButton.Select();
                }
            }
            Debug.Log($"{_crossH},{_crossV}");
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
        _gamePanels[0].SetActive(isOpen);
        if (_safeZorn)
        {
            _gamePanels[1].SetActive(isOpen);
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
            if (_crossH > _allButtons.GetLength(1)) _crossH = 0;
        }
        if(v > 0)
        {
            _crossV--;
            if (_crossV < 0) _crossV = _allButtons.GetLength(0) - 1;
        }
        if(h < 0)
        {
            _crossH--;
            if (_crossH < 0) _crossH = _allButtons.GetLength(1) - 1;
        }
        if(v < 0)
        {
            _crossV++;
            if (_crossV > _allButtons.GetLength(0)) _crossV = 0;
        }

        return _allButtons[(int)_crossV, (int)_crossH];
        
    }

    /// <summary>メニュー画面展開時に呼ぶ関数 </summary>
    void MenuOpen()
    {
        _allButtons[0, 0].Select();
    }

    /// <summary>メニュー画面縮小時に呼ぶ関数</summary>
    void MenuClose()
    {

    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class WeaponMenuHander : SingletonBehaviour<WeaponMenuHander>,IMenuHander<ICommandButton>
{
    [SerializeField, Tooltip("ボタン選択のインターバル")]
    float _interval = 0.3f;
    [SerializeField, Tooltip("メインメニューのプレハブ")] 
    WeaponMenu _mainManu = null;
    [SerializeField,Tooltip("")]
    Image[] _playerStatusImages = default;
    [SerializeField,Tooltip("")]
    Image[] _menuPanelsImages = default;


    [Tooltip("メニュー画面の開閉確認")]
    bool _menuIsOpen = false;
    [Tooltip("横入力")]
    int _crossH;
    [Tooltip("縦入力")]
    int _crossV;
    [Tooltip("横入力(ReactiveProperty)")]
    IntReactiveProperty _reactiveCrossH = new(0);
    [Tooltip("縦入力(ReactiveProperty)")]
    IntReactiveProperty _reactiveCrossV = new(0);
    [Tooltip("決定中のボタン")]
    BoolReactiveProperty _isDiside = new();
    [Tooltip("")]
    GameObject _canvas = default;
    [Tooltip("選択中のボタン")]
    ICommandButton _selectButton = null;
    
    [Tooltip("")]
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;
    public IReadOnlyReactiveProperty<int> CrossH => _reactiveCrossH;
    public IReadOnlyReactiveProperty<int> CrossV => _reactiveCrossV;
    public IReadOnlyReactiveProperty<bool> IsDiside => _isDiside;
    public ICommandButton SelectButton { get => _selectButton; set => _selectButton = value; }

    /// <summary>
    /// 起動時処理
    /// </summary>
    /// <param name="allbuttons"></param>
    public void Initialize()
    {
        _canMove = new Interval(_interval);

        //UIのゲーム起動時の初期設定
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        foreach (Image gpi in _menuPanelsImages)
        {
            gpi.enabled = false;
        }

        foreach (Image image in _playerStatusImages)
        {
            image.enabled = true;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("MenuSwitch"))
        {
            _menuIsOpen = !_menuIsOpen;
            IsManuExtend(_menuIsOpen);
        }

        if (_menuIsOpen)
        {
            float h = Input.GetAxisRaw("CrossKeyH");
            float v = Input.GetAxisRaw("CrossKeyV");
            _isDiside.Value = Input.GetButtonDown("Decision");

            if (_canMove.IsCountUp() && (h != 0 || v != 0))
            {
                SelectCommand(h, v);
            }
        }
    }
    /// <summary>
    /// メニュー画面の展開
    /// </summary>
    /// <param name="isOpen"></param>
    void IsManuExtend(bool isOpen)
    {
        //ToDo メニューの開閉にアニメーションを加える
        Array.ForEach(_menuPanelsImages, x => x.enabled = isOpen);
        Array.ForEach(_playerStatusImages, x => x.enabled = !isOpen);
        foreach (ICommandButton b in _mainManu.AllButton)
        {
            b.Command.enabled = isOpen;
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
    /// メニューのボタン操作
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public void  SelectCommand(float h, float v)
    {
        _canMove.ResetTimer();
        if (h > 0)
        {
            _crossH++;
            if (_crossH > _mainManu.AllButton.GetLength(1) - 1) _crossH = 0;
        }
        if (v < 0)
        {
            _crossV++;
            if (_crossV > _mainManu.AllButton.GetLength(0) - 1) _crossV = 0;
        }
        if (h < 0)
        {
            _crossH--;
            if (_crossH < 0) _crossH = _mainManu.AllButton.GetLength(1) - 1;
        }
        if (v > 0)
        {
            _crossV--;
            if (_crossV < 0) _crossV = _mainManu.AllButton.GetLength(0) - 1;
        }
        _reactiveCrossH.Value = _crossH;
        _reactiveCrossV.Value = _crossV;
    }
    /// <summary>
    /// メニュー画面展開時に呼ぶ関数
    /// </summary>
    void MenuOpen()
    {
        _selectButton = _mainManu.SelectButton(_reactiveCrossV.Value, _reactiveCrossH.Value);
        //_allButtons[_crossV, _crossH].Command.Select();
        //_targetButton = _allButtons[_crossV, _crossH];
    }
    /// <summary>
    /// メニュー画面縮小時に呼ぶ関数
    /// </summary>
    void MenuClose()
    {

    }

    void OnDisable()
    {
        _isDiside.Dispose();
        _reactiveCrossH.Dispose();
        _reactiveCrossV.Dispose();
    }
}
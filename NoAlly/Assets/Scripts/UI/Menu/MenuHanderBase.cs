using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MenuHanderBase : MonoBehaviour,IMenuHander<ICommandButton>
{
    [SerializeField, Tooltip("ボタン選択のインターバル")]
    float _interval = 0.3f;


    [Tooltip("メニュー画面の開閉確認")]
    bool _menuIsOpen = true;
    [Tooltip("縦入力")]
    int _crossV = 0;
    [Tooltip("縦入力(ReactiveProperty)")]
    IntReactiveProperty _reactiveCrossV = new(0);
    [Tooltip("")]
    Image[] _gameUIGauges = default;
    [Tooltip("")]
    Image[] _gamePanelsImages = default;
    [Tooltip("")]
    GameObject _canvas = default;
    [Tooltip("選択中のボタン")]
    ICommandButton _targetButton = default;
    [Tooltip("")]
    ICommandButton _selectedButtons = null;
    [Tooltip("")]
    ICommandButton[] _allButtons = null;
    [Tooltip("")]
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;

    public IReadOnlyReactiveProperty<int> CrossH => throw new NotImplementedException();

    public IReadOnlyReactiveProperty<int> CrossV => _reactiveCrossV;

    public IReadOnlyReactiveProperty<bool> IsDiside => throw new NotImplementedException();

    ICommandButton IMenuHander<ICommandButton>.SelectButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Initialize(CommandButton[] buttonArray)
    {
        _canMove = new Interval(_interval);

        //Canvas上の全てのButtonを取得する。
        _allButtons = buttonArray;

        //UIのゲーム起動時の初期設定
        _canvas = this.gameObject;
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
                ICommandButton b = SelectButton(v);
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
    /// メニュー画面の展開
    /// </summary>
    /// <param name="isOpen"></param>
    void IsManuExtend(bool isOpen)
    {
        //ToDo メニューの開閉にアニメーションを加える
        foreach (Image gpi in _gamePanelsImages)
        {
            //if (gpi.gameObject.tag == "ChackFrame") return;
            gpi.enabled = isOpen;
        }
        foreach (CommandButton b in _allButtons)
        {
            b.Command.enabled = isOpen;
        }

        foreach (Image image in _gameUIGauges)
        {
            image.enabled = !isOpen;
        }
    }
    /// <summary>
    /// メニューのボタン操作
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    ICommandButton SelectButton(float v)
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
    /// 選択されたボタンに登録された関数を実行する関数
    /// </summary>
    /// <param name="targetButton"></param>
    void DisideCommand(ICommandButton targetButton)
    {
        targetButton.Disaide(true);
    }

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public void SelectCommand(float h, float v)
    {
        throw new NotImplementedException();
    }
}

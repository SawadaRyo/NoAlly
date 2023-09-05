using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerBase : MonoBehaviour
{
    [SerializeField, Header("このメニュー画面に登録している全パネル")]
    SelectObjecArrayBase[] _allSelectObjectArraies = null;

    [Tooltip("メニュー展開判定")]
    bool _isActive = false;
    [Tooltip("")]
    int _currentMenuPanelIndex = 0;
    [Tooltip("選択中のボタン")]
    UIObjectBase _targetButton = default;
    [Tooltip("MenuPanelの初期選択画面")]
    SelectObjecArrayBase _firstSelectObjectArraies = null;
    [Tooltip("現在展開中のメニュー画面")]
    SelectObjecArrayBase _currentMenuPanel = null;
    [Tooltip("ひとつ前のメニュー画面")]
    SelectObjecArrayBase _beforeMenuPanel = null;

    public bool IsActive => _isActive;
    public List<T> GetComponentButtonList<T>(PanelType panelType) where T : UIObjectBase
    {
        foreach (var t in _allSelectObjectArraies)
        {
            if (t.PanelType == panelType)
            {
                return t.SelectChildlen<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    public void Initialize()
    {
        Array.ForEach(_allSelectObjectArraies, array => array.SetUp(null));
        _firstSelectObjectArraies = _allSelectObjectArraies[0];
        _targetButton = _firstSelectObjectArraies.SelectChildlen(0, 0);
        _currentMenuPanel = _firstSelectObjectArraies;
    }

    /// <summary>
    /// メニュー展開関数
    /// </summary>
    /// <param name="isMenuOpen">展開判定</param>
    public void IsMenuOpen()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            _currentMenuPanel.MenuExtended();
            Array.ForEach(_currentMenuPanel.Childlen, childlen =>
            {
                Array.ForEach(childlen.ChildArrays, x => x.MenuExtended());
            });
            _targetButton = _firstSelectObjectArraies.SelectChildlen();
        }
        else
        {
            _currentMenuPanelIndex = 0;
            _targetButton.IsSelect(false);
            _targetButton = null;
            _currentMenuPanel.MenuClosed();
            _currentMenuPanel = _firstSelectObjectArraies;
        }
    }

    /// <summary>
    /// ボタン選択
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SelectTargetButton(int x, int y)
    {
        if (_isActive)
        {
            _targetButton.IsSelect(false);
            _targetButton = _currentMenuPanel.SelectChildlen(x, y);
            _targetButton.IsSelect(true);
        }
    }

    public void SwitchPanal(int inputTrigger)
    {
        if (_isActive)
        {
            _currentMenuPanelIndex += inputTrigger;
            if (_currentMenuPanelIndex < 0)
            {
                _currentMenuPanelIndex = _allSelectObjectArraies.Length - 1;
            }
            else if (_currentMenuPanelIndex >= _allSelectObjectArraies.Length)
            {
                _currentMenuPanelIndex = 0;
            }
            _beforeMenuPanel = null;
            _currentMenuPanel.MenuClosed();
            _currentMenuPanel = _allSelectObjectArraies[_currentMenuPanelIndex];
            _currentMenuPanel.MenuExtended();
            Array.ForEach(_currentMenuPanel.Childlen, childlen =>
            {
                Array.ForEach(childlen.ChildArrays, x => x.MenuExtended());
            });
            _targetButton = _currentMenuPanel.SelectChildlen();
        }
    }

    /// <summary>
    /// 決定時間数
    /// </summary>
    public void OnDisaide()
    {
        if (!_isActive) return;
        if (_targetButton is SelectObjecArrayBase selectObjecArray)
        {
            _targetButton.IsSelect(false); //直前まで展開していた画面/ボタンを閉じる
            if (_targetButton.imageDisactiveDoEvent)
            {
                _targetButton.ActiveUIObject(false);
            }
            _beforeMenuPanel = selectObjecArray.Perent; //ひとつ前の画面/ボタンを指定
            _currentMenuPanel = selectObjecArray; //現在の画面/ボタンを指定
            Array.ForEach(_currentMenuPanel.Childlen, childlen => Array.ForEach(childlen.ChildArrays, x => x.ActiveUIObject(true))); //子オブジェクトを表示

            if (_currentMenuPanel.ButtonTween)
            {
                _currentMenuPanel.ButtonTween.ExtendsButton(true).Forget();
            }
            _targetButton = _currentMenuPanel.SelectChildlen(); //現在の画面/ボタンを選択
        }
        if (_targetButton.B)
        {
            _targetButton.ClickEvent();
        }
    }

    /// <summary>
    /// キャンセル関数
    /// </summary>
    public async void OnCansel()
    {
        if (!_isActive) return;

        if (_beforeMenuPanel == null)
        {
            IsMenuOpen();
        }
        else
        {
            if (_currentMenuPanel.ButtonTween) //Tweenアニメーションが終了するまで待機するための処理
            {
                var flag = false;
                if (await _currentMenuPanel.ButtonTween.ExtendsButton(false))
                {
                    flag = true;
                }
                await UniTask.WaitUntil(() => flag);
            }
            _currentMenuPanel.IsSelect(false);
            _currentMenuPanel.ClickEvent();
            Array.ForEach(_currentMenuPanel.Childlen, childlen =>
            {
                Array.ForEach(childlen.ChildArrays, x => x.ActiveUIObject(false));
            });
            _currentMenuPanel = _beforeMenuPanel;


            if (_currentMenuPanel.Perent
             && _currentMenuPanel.Perent is SelectObjecArrayBase selectObjecPerent)
            {
                _beforeMenuPanel = selectObjecPerent;
            }
            _targetButton = _currentMenuPanel.SelectChildlen();
            if (_targetButton.imageDisactiveDoEvent)
            {
                _targetButton.ActiveUIObject(true);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DimensionalArray;
using System;

public class SelectObjecArrayBase : UIObjectBase
{
    [SerializeField, Header("このオブジェクトのメニュー深度")]
    int _depthOfMenu = 0;
    [SerializeField, Header("このオブジェクトの展開時のTween")]
    ButtonArrayExtend _buttonArrayExtend = null;
    [SerializeField, Header("このオブジェクトの子関係にあるボタンオブジェクト")]
    protected GenericArray<UIObjectBase>[] _childlenArray = null;

    [Tooltip("")]
    (int, int) _currentCross = (0, 0);

    public int DepthOfMenu => _depthOfMenu;
    public GenericArray<UIObjectBase>[] Childlen => _childlenArray;
    public ButtonArrayExtend ButtonTween => _buttonArrayExtend;

    public override void SetUp(SelectObjecArrayBase perent)
    {
        base.SetUp(perent);
        SetButtonEvent();
        for (int y = 0; y < _childlenArray.Length; y++)
        {
            for (int x = 0; x < _childlenArray[y].ChildArrays.Length; x++)
            {
                _childlenArray[y].ChildArrays[x].SetUp(this);
            }
        }
    }
    public override void MenuClosed()
    {
        base.MenuClosed();
        Array.ForEach(_childlenArray, childlen =>
        {
            Array.ForEach(childlen.ChildArrays, x => x.MenuClosed());
        });
    }
    public UIObjectBase SelectChildlen()
    {
        _childlenArray[_currentCross.Item2].ChildArrays[_currentCross.Item1].IsSelect(true);
        return _childlenArray[_currentCross.Item2].ChildArrays[_currentCross.Item1];
    }
    public UIObjectBase SelectChildlen(int x, int y)
    {
        if (x < 0)
        {
            _currentCross.Item1--;
            if (_currentCross.Item1 < 0)
            {
                _currentCross.Item1 = _childlenArray[y].ChildArrays.Length - 1;
            }
        }
        else if (x > 0)
        {
            _currentCross.Item1++;
            if (_currentCross.Item1 >= _childlenArray[y].ChildArrays.Length)
            {
                _currentCross.Item1 = 0;
            }

        }
        else if (y < 0)
        {
            _currentCross.Item2++;
            if (_currentCross.Item2 >= _childlenArray.Length)
            {
                _currentCross.Item2 = 0;
            }
        }
        else if (y > 0)
        {
            _currentCross.Item2--;
            if (_currentCross.Item2 < 0)
            {
                _currentCross.Item2 = _childlenArray.Length - 1;
            }
        }
        return _childlenArray[_currentCross.Item2].ChildArrays[_currentCross.Item1];
    }

    /// <summary>
    /// ボタンオブジェクト検索(実行時に使用推奨)
    /// </summary>
    /// <typeparam name="T">検索するボタンオブジェクトの型</typeparam>
    /// <returns>このクラスの子オブジェクトにある検索対象の配列</returns>
    public List<T> SelectChildlen<T>()
        where T : UIObjectBase
    {
        List<T> result = new();
        foreach (var childlen in _childlenArray)
        {
            for(int i = 0;i < childlen.ChildArrays.Length;i++)
            {
                if (childlen.ChildArrays[i] is T r)
                {
                    result.Add(r);
                }
                else if (childlen.ChildArrays[i] is SelectObjecArrayBase child)
                {
                    result.AddRange(child.SelectChildlen<T>());
                }
            }
        }
        return result;
    }
}

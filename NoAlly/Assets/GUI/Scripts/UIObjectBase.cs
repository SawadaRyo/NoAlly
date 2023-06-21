using System;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectBase : MonoBehaviour, ISelectObject
{
    [SerializeField, Header("オブジェクトのImage")]
    protected Image[] _objectImage = null;
    [SerializeField, Header("オブジェクトのAnimator")]
    protected Animator _objectAnimator = null;
    [SerializeField, Header("決定時のイベント")]
    protected Button _event;
    [SerializeField, Header("決定時自分のImageを消すか")]
    bool _imageDisactiveDoEvent = false;

    [Tooltip("ボタンの親関係")]
    protected SelectObjecArrayBase _perent = null;
    [Tooltip("ボタンの状態")]
    protected ButtonState _state = ButtonState.NONE;

    public bool imageDisactiveDoEvent => _imageDisactiveDoEvent;
    public SelectObjecArrayBase Perent => _perent;
    public Button Event => _event;

    /// <summary>
    /// オブジェクトの表示
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void ActiveUIObject(bool isActive)
    {
        if (_objectAnimator != null)
        {
            _objectAnimator.enabled = isActive;
        }
        if (_objectImage != null)
        {
            Array.ForEach(_objectImage, x => x.enabled = isActive);
        }
    }

    public virtual void Initialize(SelectObjecArrayBase perent)
    {
        _perent = perent;
        ActiveUIObject(false);
    }
    public virtual void IsSelect(bool isSelect)
    {
        if (!_event) return;
        if (isSelect)
        {
            if (_state != ButtonState.DISIDED)
            {
                _state = ButtonState.SELECTED;
            }
        }
        else
        {
            switch (_state)
            {
                case ButtonState.SELECTED:
                    if (_event.image)
                    {
                        //選択時
                    }
                    _state = ButtonState.NONE;
                    break;
                case ButtonState.DISIDED:
                    if (_event.image)
                    {
                        //決定時
                    }
                    break;
                default:
                    break;
            }
        }
        _objectAnimator.SetBool("IsSelect", isSelect);
    }
    public void DoEvent(bool isDoEvent)
    {
        if (isDoEvent)
        {
            _event.onClick.Invoke();
        }
    }
    public virtual void MenuExtended()
    {
        ActiveUIObject(true);
    }
    public virtual void MenuClosed()
    {
        ActiveUIObject(false);
    }
}

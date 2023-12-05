using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerBase : MonoBehaviour
{
    [SerializeField, Header("���̃��j���[��ʂɓo�^���Ă���S�p�l��")]
    SelectObjecArrayBase[] _allSelectObjectArraies = null;

    [Tooltip("���j���[�W�J����")]
    bool _isActive = false;
    [Tooltip("")]
    int _currentMenuPanelIndex = 0;
    [Tooltip("�I�𒆂̃{�^��")]
    UIObjectBase _targetButton = default;
    [Tooltip("MenuPanel�̏����I�����")]
    SelectObjecArrayBase _firstSelectObjectArraies = null;
    [Tooltip("���ݓW�J���̃��j���[���")]
    SelectObjecArrayBase _currentMenuPanel = null;
    [Tooltip("�ЂƂO�̃��j���[���")]
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
    /// �������֐�
    /// </summary>
    public void Initialize()
    {
        Array.ForEach(_allSelectObjectArraies, array => array.SetUp(null));
        _firstSelectObjectArraies = _allSelectObjectArraies[0];
        _targetButton = _firstSelectObjectArraies.SelectChildlen(0, 0);
        _currentMenuPanel = _firstSelectObjectArraies;
    }

    /// <summary>
    /// ���j���[�W�J�֐�
    /// </summary>
    /// <param name="isMenuOpen">�W�J����</param>
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
    /// �{�^���I��
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
    /// ���莞�Ԑ�
    /// </summary>
    public void OnDisaide()
    {
        if (!_isActive) return;
        if (_targetButton is SelectObjecArrayBase selectObjecArray)
        {
            _targetButton.IsSelect(false); //���O�܂œW�J���Ă������/�{�^�������
            if (_targetButton.imageDisactiveDoEvent)
            {
                _targetButton.ActiveUIObject(false);
            }
            _beforeMenuPanel = selectObjecArray.Perent; //�ЂƂO�̉��/�{�^�����w��
            _currentMenuPanel = selectObjecArray; //���݂̉��/�{�^�����w��
            Array.ForEach(_currentMenuPanel.Childlen, childlen => Array.ForEach(childlen.ChildArrays, x => x.ActiveUIObject(true))); //�q�I�u�W�F�N�g��\��

            if (_currentMenuPanel.ButtonTween)
            {
                _currentMenuPanel.ButtonTween.ExtendsButton(true).Forget();
            }
            _targetButton = _currentMenuPanel.SelectChildlen(); //���݂̉��/�{�^����I��
        }
        if (_targetButton.B)
        {
            _targetButton.ClickEvent();
        }
    }

    /// <summary>
    /// �L�����Z���֐�
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
            if (_currentMenuPanel.ButtonTween) //Tween�A�j���[�V�������I������܂őҋ@���邽�߂̏���
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

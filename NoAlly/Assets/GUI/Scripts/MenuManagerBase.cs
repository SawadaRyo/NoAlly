using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerBase : MonoBehaviour
{
    [SerializeField, Header("MenuPanel�̏����I�����")]
    SelectObjecArrayBase _firstSelectObjects = null;

    [Tooltip("���j���[�W�J����")]
    bool _isActive = false;
    [Tooltip("�I�𒆂̃{�^��")]
    UIObjectBase _targetButton = default;
    [Tooltip("���ݓW�J���̃��j���[���")]
    SelectObjecArrayBase _currentMenuPanel = null;
    [Tooltip("�ЂƂO�̃��j���[���")]
    SelectObjecArrayBase _beforeMenuPanel = null;

    public bool isActive => _isActive;
    public List<T> GetComponentButtonList<T>() where T : UIObjectBase => _firstSelectObjects.SelectChildlen<T>();

    /// <summary>
    /// �������֐�
    /// </summary>
    public void Initialize()
    {
        _firstSelectObjects.Initialize(null);
        _targetButton = _firstSelectObjects.SelectChildlen(0, 0);
        _currentMenuPanel = _firstSelectObjects;
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
            _firstSelectObjects.MenuExtended();
            Array.ForEach(_firstSelectObjects.Childlen, childlen =>
            {
                Array.ForEach(childlen.ChildArrays, x => x.MenuExtended());
            });
            _targetButton = _firstSelectObjects.SelectChildlen();
        }
        else
        {
            _targetButton.IsSelect(false);
            _targetButton = null;
            _currentMenuPanel = _firstSelectObjects;
            _firstSelectObjects.MenuClosed();
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
        if (_targetButton.Event)
        {
            _targetButton.Disided();
        }
    }

    /// <summary>
    /// �L�����Z���֐�
    /// </summary>
    public async void OnCansel()
    {
        if (!_isActive) return;

        if (_currentMenuPanel == _firstSelectObjects)
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
            _currentMenuPanel.Disided();
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

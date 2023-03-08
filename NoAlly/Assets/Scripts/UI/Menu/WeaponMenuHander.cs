using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class WeaponMenuHander : SingletonBehaviour<WeaponMenuHander>,IMenuHander<ICommandButton>
{
    [SerializeField, Tooltip("�{�^���I���̃C���^�[�o��")]
    float _interval = 0.3f;
    [SerializeField, Tooltip("���C�����j���[�̃v���n�u")] 
    WeaponMenu _mainManu = null;
    [SerializeField,Tooltip("")]
    Image[] _playerStatusImages = default;
    [SerializeField,Tooltip("")]
    Image[] _menuPanelsImages = default;


    [Tooltip("���j���[��ʂ̊J�m�F")]
    bool _menuIsOpen = false;
    [Tooltip("������")]
    int _crossH;
    [Tooltip("�c����")]
    int _crossV;
    [Tooltip("������(ReactiveProperty)")]
    IntReactiveProperty _reactiveCrossH = new(0);
    [Tooltip("�c����(ReactiveProperty)")]
    IntReactiveProperty _reactiveCrossV = new(0);
    [Tooltip("���蒆�̃{�^��")]
    BoolReactiveProperty _isDiside = new();
    [Tooltip("")]
    GameObject _canvas = default;
    [Tooltip("�I�𒆂̃{�^��")]
    ICommandButton _selectButton = null;
    
    [Tooltip("")]
    Interval _canMove = default;

    public bool MenuIsOpen => _menuIsOpen;
    public IReadOnlyReactiveProperty<int> CrossH => _reactiveCrossH;
    public IReadOnlyReactiveProperty<int> CrossV => _reactiveCrossV;
    public IReadOnlyReactiveProperty<bool> IsDiside => _isDiside;
    public ICommandButton SelectButton { get => _selectButton; set => _selectButton = value; }

    /// <summary>
    /// �N��������
    /// </summary>
    /// <param name="allbuttons"></param>
    public void Initialize()
    {
        _canMove = new Interval(_interval);

        //UI�̃Q�[���N�����̏����ݒ�
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
    /// ���j���[��ʂ̓W�J
    /// </summary>
    /// <param name="isOpen"></param>
    void IsManuExtend(bool isOpen)
    {
        //ToDo ���j���[�̊J�ɃA�j���[�V������������
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
    /// ���j���[�̃{�^������
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
    /// ���j���[��ʓW�J���ɌĂԊ֐�
    /// </summary>
    void MenuOpen()
    {
        _selectButton = _mainManu.SelectButton(_reactiveCrossV.Value, _reactiveCrossH.Value);
        //_allButtons[_crossV, _crossH].Command.Select();
        //_targetButton = _allButtons[_crossV, _crossH];
    }
    /// <summary>
    /// ���j���[��ʏk�����ɌĂԊ֐�
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
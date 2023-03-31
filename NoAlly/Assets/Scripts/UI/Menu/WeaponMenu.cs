using DataOfWeapon;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ����E�G���`�����g��ύX���邽�߂̃R���|�[�l���g
/// </summary>

public class WeaponMenu : MonoBehaviour, IMenu<ICommandButton>
{
    [SerializeField, Header("WeaponScriptableObjects�{��")]
    WeaponScriptableObjects _weaponScriptableObjects;
    [SerializeField,Tooltip("SetWeaponData")]
    SetWeaponData _weaponData = null;
    [SerializeField, Header("���C�����j���[���̑S�Ẵ{�^��")]
    CommandButton[] _allButton = null;

    [Tooltip("�������̕���Ƒ���")]
    ReactiveProperty<WeaponData> _mainWeapon = new ReactiveProperty<WeaponData>();
    ReactiveProperty<WeaponData> _subWeapon = new ReactiveProperty<WeaponData>();
    ReactiveProperty<ElementType> _elementType = new ReactiveProperty<ElementType>();
    [Tooltip("")]
    ICommandButton[] _selectedButtons = new ICommandButton[Enum.GetNames(typeof(CommandType)).Length];

    public IReadOnlyReactiveProperty<ElementType> Element => _elementType;
    public IReadOnlyReactiveProperty<WeaponData> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponData> SubWeapon => _subWeapon;
    public ICommandButton SelectButton(int crossH, int crossV) => AllButton[crossV, crossH];
    public ICommandButton[,] AllButton { get; private set; }
    public ICommandButton[] SelectedButtons { get => _selectedButtons; set => _selectedButtons = value; }

    /// <summary>
    /// �N�����ɌĂԊ֐�
    /// </summary>
    /// <param name="weaponData"></param>
    public void Initialize()
    {
        _weaponData.Initialize(_weaponScriptableObjects);
        AllButton = new ICommandButton[Enum.GetNames(typeof(CommandType)).Length, Enum.GetNames(typeof(WeaponType)).Length - 1];
        int indexX = AllButton.GetLength(1);
        int indexY = AllButton.GetLength(0);
        ICommandButton[] allButton = _allButton
            .Where(x => x.TryGetComponent(out ICommandButton button))
            .Select(x => (ICommandButton)x).ToArray();
        SetButtonMap(allButton, indexX, indexY);
    }
    /// <summary> ���������؂�ւ���</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void EquipmentWeapon(CommandType type, WeaponType weaponName)
    {
        WeaponData beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _mainWeapon.Value;
            _mainWeapon.Value =_weaponData.GetWeapon(weaponName);
            if (_subWeapon.Value == _mainWeapon.Value) //Main��Sub�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _subWeapon.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _subWeapon.Value;
            _subWeapon.Value =_weaponData.GetWeapon(weaponName);
            if (_mainWeapon.Value == _subWeapon.Value) //Sub��Main�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _mainWeapon.Value = beforeWeapons;
            }
        }
    }
    /// <summary>
    /// ������؂�ւ���
    /// </summary>
    /// <param name="element"></param>
    public void EquipmentElement(int element)
    {
        ElementType e = (ElementType)element;
        _elementType.Value = e;
    }
    
    /// <summary>
    /// ���j���[���̃{�^���z�������������֐�
    /// </summary>
    /// <param name="allButtons"></param>
    /// <param name="indexX"></param>
    /// <param name="indexY"></param>
    public void SetButtonMap(ICommandButton[] allButtons, int indexX, int indexY)
    {
        for (int y = 0; y < indexY; y++)
        {
            ICommandButton[] buttonArray = allButtons.Where(b => b.TypeOfCommand == (CommandType)y).ToArray();
            for (int x = 0; x < indexX; x++)
            {
                int index = x;
                switch ((CommandType)y)
                {
                    case CommandType.MAIN:
                        buttonArray[x].Command.onClick.AddListener(() => EquipmentWeapon(CommandType.MAIN, (WeaponType)index));
                        break;
                    case CommandType.SUB:
                        buttonArray[x].Command.onClick.AddListener(() => EquipmentWeapon(CommandType.SUB, (WeaponType)index));
                        break;
                    case CommandType.ELEMENT:
                        //buttonArray[x].Command.onClick.AddListener(() =>
                        //{
                        //    ElementType element = (ElementType)x;
                        //    Debug.Log(element);
                        //    _weaponMethods.ForEach(x =>
                        //    {
                        //        x.WeaponMode(element);
                        //    });
                        //    _elementType = element;
                        //});
                        break;
                }
                AllButton[y, x] = buttonArray[x];
                AllButton[y, x].Command.enabled = false;
            }
        }
        AllButton[(int)CommandType.MAIN, (int)WeaponType.SWORD].Disaide(true);
        _selectedButtons[(int)CommandType.MAIN] = AllButton[(int)CommandType.MAIN, (int)WeaponType.SWORD];
        AllButton[(int)CommandType.SUB, (int)WeaponType.LANCE].Disaide(true);
        _selectedButtons[(int)CommandType.SUB] = AllButton[(int)CommandType.SUB, (int)WeaponType.LANCE];
        AllButton[(int)CommandType.ELEMENT, (int)ElementType.RIGIT].Disaide(true);
        _selectedButtons[(int)CommandType.ELEMENT] = AllButton[(int)CommandType.ELEMENT, (int)ElementType.RIGIT];
    }
    private void OnDisable()
    {
        _mainWeapon.Dispose();
        _subWeapon.Dispose();
        _elementType.Dispose();
    }
}




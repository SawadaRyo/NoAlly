using DataOfWeapon;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ����E�G���`�����g��ύX���邽�߂̃R���|�[�l���g
/// </summary>

public class WeaponEquipment : MonoBehaviour, IMenu<ICommandButton>
{
    [SerializeField, Header("���C�����j���[���̑S�Ẵ{�^��")]
    CommandButton[] _allButton = null;

    [Tooltip("����̃f�[�^")]
    SetWeaponData _weaponData = null;
    [Tooltip("����̋@�\")]
    List<IWeapon> _weaponMethods = new();
    [Tooltip("�������̕���Ƒ���")]
    ReactiveProperty<WeaponDatas> _mainWeapon = new ReactiveProperty<WeaponDatas>();
    ReactiveProperty<WeaponDatas> _subWeapon = new ReactiveProperty<WeaponDatas>();
    ElementType _elementType = default;
    [Tooltip("")]
    ICommandButton[] _selectedButtons = new ICommandButton[Enum.GetNames(typeof(CommandType)).Length];

    public SetWeaponData Data => _weaponData;
    public ElementType Element => _elementType;
    public IReadOnlyReactiveProperty<WeaponDatas> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponDatas> SubWeapon => _subWeapon;
    public ICommandButton SelectButton(int crossH, int crossV) => AllButton[crossV, crossH];
    public ICommandButton[,] AllButton { get; private set; }
    public ICommandButton[] SelectedButtons { get => _selectedButtons; set => _selectedButtons = value; }

    /// <summary>
    /// �N�����ɌĂԊ֐�
    /// </summary>
    /// <param name="weaponData"></param>
    public void Initialize()
    {
        AllButton = new ICommandButton[Enum.GetNames(typeof(CommandType)).Length, Enum.GetNames(typeof(WeaponType)).Length - 1];
        int indexX = AllButton.GetLength(1);
        int indexY = AllButton.GetLength(0);
        ICommandButton[] allButton = _allButton
            .Where(x => x.TryGetComponent(out ICommandButton button))
            .Select(x => (ICommandButton)x).ToArray();
        SetButtonMap(allButton, indexX, indexY);
    }
    /// <summary>
    /// �����������B���֐�
    /// </summary>
    /// <returns></returns>
    public (WeaponDatas, WeaponDatas) FirstSetWeapon(SetWeaponData weaponData)
    {
        _weaponData = weaponData;
        _mainWeapon.Value = _weaponData.GetWeapon(WeaponType.SWORD);
        _subWeapon.Value = _weaponData.GetWeapon(WeaponType.LANCE);
        return (_mainWeapon.Value, _subWeapon.Value);
    }
    /// <summary> ���������؂�ւ���</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDatas weaponName)
    {
        WeaponDatas beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _mainWeapon.Value;
            _mainWeapon.Value = weaponName;
            if (_subWeapon.Value.Type == _mainWeapon.Value.Type) //Main��Sub�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _subWeapon.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _subWeapon.Value;
            _subWeapon.Value = weaponName;
            if (_mainWeapon.Value.Type == _subWeapon.Value.Type) //Sub��Main�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _mainWeapon.Value = beforeWeapons;
            }
        }
    }
    /// <summary>
    /// ������؂�ւ���
    /// </summary>
    /// <param name="element"></param>

    /// <summary>
    /// �g�p���̕�����w�肷��֐�
    /// </summary>
    /// <param name="weaponEnabled">���݂̕���̎g�p��</param>
    /// <returns></returns>
    public WeaponDatas CheckWeaponActive((bool, bool) weaponEnabled)
    {
        if (weaponEnabled.Item1)
        {
            return _mainWeapon.Value;
        }
        return _subWeapon.Value;
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
                WeaponDatas weapon = _weaponData.GetWeapon((WeaponType)x);
                _weaponMethods.Add(weapon.Base);
                switch ((CommandType)y)
                {
                    case CommandType.MAIN:
                        buttonArray[x].Command.onClick.AddListener(() => Equipment(CommandType.MAIN, weapon));
                        break;
                    case CommandType.SUB:
                        buttonArray[x].Command.onClick.AddListener(() => Equipment(CommandType.SUB, weapon));
                        break;
                    case CommandType.ELEMENT:
                        buttonArray[x].Command.onClick.AddListener(() =>
                        {
                            ElementType element = (ElementType)x;
                            Debug.Log(element);
                            _weaponMethods.ForEach(x =>
                            {
                                x.WeaponMode(element);
                            });
                            _elementType = element;
                        });
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
    }
}




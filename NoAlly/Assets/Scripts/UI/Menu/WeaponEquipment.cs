using DataOfWeapon;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 武器・エンチャントを変更するためのコンポーネント
/// </summary>

public class WeaponEquipment : MonoBehaviour, IMenu<ICommandButton>
{
    [SerializeField, Header("メインメニュー内の全てのボタン")]
    CommandButton[] _allButton = null;

    [Tooltip("武器のデータ")]
    SetWeaponData _weaponData = null;
    [Tooltip("武器の機能")]
    List<IWeapon> _weaponMethods = new();
    [Tooltip("装備中の武器と属性")]
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
    /// 起動時に呼ぶ関数
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
    /// 装備を初期隠す関数
    /// </summary>
    /// <returns></returns>
    public (WeaponDatas, WeaponDatas) FirstSetWeapon(SetWeaponData weaponData)
    {
        _weaponData = weaponData;
        _mainWeapon.Value = _weaponData.GetWeapon(WeaponType.SWORD);
        _subWeapon.Value = _weaponData.GetWeapon(WeaponType.LANCE);
        return (_mainWeapon.Value, _subWeapon.Value);
    }
    /// <summary> 装備武器を切り替える</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void Equipment(CommandType type, WeaponDatas weaponName)
    {
        WeaponDatas beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _mainWeapon.Value;
            _mainWeapon.Value = weaponName;
            if (_subWeapon.Value.Type == _mainWeapon.Value.Type) //MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _subWeapon.Value = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _subWeapon.Value;
            _subWeapon.Value = weaponName;
            if (_mainWeapon.Value.Type == _subWeapon.Value.Type) //SubとMainの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _mainWeapon.Value = beforeWeapons;
            }
        }
    }
    /// <summary>
    /// 属性を切り替える
    /// </summary>
    /// <param name="element"></param>

    /// <summary>
    /// 使用中の武器を指定する関数
    /// </summary>
    /// <param name="weaponEnabled">現在の武器の使用状況</param>
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
    /// メニュー内のボタン配列を初期化する関数
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




using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 子オブジェクトのボタンの処理を管理するクラス
/// </summary>

public class Equipment : SelectObjecArrayBase
{
    [SerializeField]
    CommandType _commandType = CommandType.NONE;
    [SerializeField]
    Image _weaponImage;


    [Tooltip("装備中の武器と属性")]
    (WeaponType, WeaponType, ElementType) _isEquipment = new();
    ReactiveProperty<WeaponType> _mainWeapon = new();
    ReactiveProperty<WeaponType> _subWeapon = new();
    ReactiveProperty<ElementType> _elementType = new();
    BoolReactiveProperty _equiped = new();

    public IReadOnlyReactiveProperty<WeaponType> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponType> SubWeapon => _subWeapon;
    public IReadOnlyReactiveProperty<ElementType> Element => _elementType;
    public IReadOnlyReactiveProperty<bool> Equiped => _equiped;

    

    /// <summary>
    /// 子オブジェクトのボタンイベントをそれぞれ設定する
    /// </summary>
    protected override void SetButtonEvent()
    {
        for (int y = 0; y < _childlenArray.Length; y++)
        {
            for (int x = 0; x < _childlenArray[y].ChildArrays.Length; x++)
            {
                int index = x;
                WeaponSelect weaponSelect = _childlenArray[y].ChildArrays[x] as WeaponSelect;
                switch (_commandType)
                {
                    case CommandType.MAIN:
                        weaponSelect.Event.onClick.AddListener(() => EquipmentWeapon(weaponSelect,CommandType.MAIN, (WeaponType)index));
                        break;
                    case CommandType.SUB:
                        weaponSelect.Event.onClick.AddListener(() => EquipmentWeapon(weaponSelect,CommandType.SUB, (WeaponType)index));
                        break;
                    case CommandType.ELEMENT:
                        weaponSelect.Event.onClick.AddListener(() => EquipmentElement(weaponSelect,(ElementType)index));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    

    /// <summary> 装備武器を切り替える</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void EquipmentWeapon(WeaponSelect weaponSelect, CommandType type, WeaponType weaponName)
    {
        WeaponType beforeWeapons = default;
        if (type == CommandType.MAIN)
        {
            beforeWeapons = _isEquipment.Item1;
            _isEquipment.Item1 = weaponName;
            if (_subWeapon.Value == _mainWeapon.Value) //MainとSubの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _isEquipment.Item2 = beforeWeapons;
            }
        }
        else if (type == CommandType.SUB)
        {
            beforeWeapons = _isEquipment.Item2;
            _isEquipment.Item2 = weaponName;
            if (_isEquipment.Item1 == _isEquipment.Item2) //SubとMainの装備武器が同じだった場合それぞれの装備武器を入れ替える
            {
                _isEquipment.Item1 = beforeWeapons;
            }
        }
        _weaponImage.sprite = weaponSelect.SwitchImage;
    }
    /// <summary>
    /// 属性を切り替える
    /// </summary>
    /// <param name="element"></param>
    public void EquipmentElement(WeaponSelect weaponSelect, ElementType element)
    {
        _isEquipment.Item3 = element;
        _weaponImage.sprite = weaponSelect.SwitchImage;
    }
    public override void MenuClosed()
    {
        base.MenuClosed();
        switch (_commandType)
        {
            case CommandType.MAIN:
                _mainWeapon.Value = _isEquipment.Item1;
                break;
            case CommandType.SUB:
                _subWeapon.Value = _isEquipment.Item2;
                break;
            case CommandType.ELEMENT:
                _equiped.SetValueAndForceNotify(true);
                _elementType.Value = _isEquipment.Item3;
                break;
            default:
                break;
        }
        _equiped.SetValueAndForceNotify(true);
    }
    private void OnDisable()
    {
        _mainWeapon.Dispose();
        _subWeapon.Dispose();
        _elementType.Dispose();
    }
}

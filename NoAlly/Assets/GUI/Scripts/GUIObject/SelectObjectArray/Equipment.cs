using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �q�I�u�W�F�N�g�̃{�^���̏������Ǘ�����N���X
/// </summary>

public class Equipment : SelectObjecArrayBase
{
    [SerializeField]
    EquipmentType _commandType = EquipmentType.NONE;
    [SerializeField]
    Image _weaponImage;


    [Tooltip("�������̕���Ƒ���")]
    (WeaponType, WeaponType, ElementType) _isEquipment = new();
    ReactiveProperty<WeaponType> _mainWeapon = new();
    ReactiveProperty<WeaponType> _subWeapon = new();
    ReactiveProperty<ElementType> _elementType = new();
    BoolReactiveProperty _equiped = new();

    [Tooltip("")]
    Sprite[] _weaponSprites = null;
    [Tooltip("")]
    Sprite[] _elementSprites = null;

    public IReadOnlyReactiveProperty<WeaponType> MainWeapon => _mainWeapon;
    public IReadOnlyReactiveProperty<WeaponType> SubWeapon => _subWeapon;
    public IReadOnlyReactiveProperty<ElementType> Element => _elementType;
    public IReadOnlyReactiveProperty<bool> Equiped => _equiped;
    public EquipmentType commandType => _commandType;

    

    /// <summary>
    /// �q�I�u�W�F�N�g�̃{�^���C�x���g�����ꂼ��ݒ肷��
    /// </summary>
    protected override void SetButtonEvent()
    {
        _weaponSprites = new Sprite[4];
        _elementSprites = new Sprite[4];
        for (int y = 0; y < _childlenArray.Length; y++)
        {
            for (int x = 0; x < _childlenArray[y].ChildArrays.Length; x++)
            {
                int index = x;
                WeaponSelect weaponSelect = _childlenArray[y].ChildArrays[x] as WeaponSelect;
                _weaponSprites[x] = weaponSelect.WeaponSprite;
                weaponSelect.SetStatus(_commandType, (WeaponType)index);
                switch (_commandType)
                {
                    case EquipmentType.MAIN:
                        weaponSelect.B.onClick.AddListener(() => EquipmentWeapon(EquipmentType.MAIN, (WeaponType)index));
                        break;
                    case EquipmentType.SUB:
                        weaponSelect.B.onClick.AddListener(() => EquipmentWeapon(EquipmentType.SUB, (WeaponType)index));
                        break;
                    case EquipmentType.ELEMENT:
                        weaponSelect.B.onClick.AddListener(() => EquipmentElement((ElementType)index));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    

    /// <summary> ���������؂�ւ���</summary>
    /// <param name="weaponName"></param>
    /// <param name="type"></param>
    public void EquipmentWeapon(EquipmentType type, WeaponType weaponName)
    {
        WeaponType beforeWeapons = default;
        if (type == EquipmentType.MAIN)
        {
            beforeWeapons = _isEquipment.Item1;
            _isEquipment.Item1 = weaponName;
            if (_subWeapon.Value == _mainWeapon.Value) //Main��Sub�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _isEquipment.Item2 = beforeWeapons;
            }
        }
        else if (type == EquipmentType.SUB)
        {
            beforeWeapons = _isEquipment.Item2;
            _isEquipment.Item2 = weaponName;
            if (_isEquipment.Item1 == _isEquipment.Item2) //Sub��Main�̑������킪�����������ꍇ���ꂼ��̑�����������ւ���
            {
                _isEquipment.Item1 = beforeWeapons;
            }
        }
        _weaponImage.sprite = _weaponSprites[(int)weaponName];
    }
    /// <summary>
    /// ������؂�ւ���
    /// </summary>
    /// <param name="element"></param>
    public void EquipmentElement(ElementType element)
    {
        _isEquipment.Item3 = element;
        _weaponImage.sprite = _weaponSprites[(int)element];
    }
    public override void MenuClosed()
    {
        base.MenuClosed();
        switch (_commandType)
        {
            case EquipmentType.MAIN:
                _mainWeapon.Value = _isEquipment.Item1;
                break;
            case EquipmentType.SUB:
                _subWeapon.Value = _isEquipment.Item2;
                break;
            case EquipmentType.ELEMENT:
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

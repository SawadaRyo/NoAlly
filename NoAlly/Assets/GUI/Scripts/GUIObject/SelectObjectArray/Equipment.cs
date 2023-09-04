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
        if (type == EquipmentType.MAIN)
        {
            _mainWeapon.Value = weaponName;
        }
        else if (type == EquipmentType.SUB)
        {
            _subWeapon.Value = weaponName;
        }
        _weaponImage.sprite = _weaponSprites[(int)weaponName];
    }
    /// <summary>
    /// ������؂�ւ���
    /// </summary>
    /// <param name="element"></param>
    public void EquipmentElement(ElementType element)
    {
        _elementType.Value = element;
        _weaponImage.sprite = _weaponSprites[(int)element];
    }
    public override void MenuClosed()
    {
        base.MenuClosed();
        _equiped.SetValueAndForceNotify(true);
    }
    private void OnDisable()
    {
        _mainWeapon.Dispose();
        _subWeapon.Dispose();
        _elementType.Dispose();
    }
}

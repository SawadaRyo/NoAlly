using UnityEngine;
using UniRx;
using DataOfWeapon;

public class WeaponPresenter : MonoBehaviour
{
    [SerializeField, Header("WeaponScriptableObjectsñ{ëÃ")]
    WeaponScriptableObjects _weaponScriptableObjects;

    [Space(15)]
    [Header("Model")]
    [SerializeField]
    SetWeaponData _weaponDatas = null;
    [SerializeField, Header("WeaponEquipmentÇäiî[Ç∑ÇÈä÷êî")]
    Equipment[] _weaponEquipment = null;
    [SerializeField]
    WeaponInput _weaponInput = null;
    ReactiveCollection<Equipment> _equipment = new();

    [Space(15)]
    [Header("View")]
    [SerializeField, Header("WeaponProcessingÇäiî[Ç∑ÇÈä÷êî")]
    WeaponProcessing _weaponProcessing = null;
    [SerializeField, Header("")]
    //WeaponElementColor _weaponElementColor = null;

    bool _isSwitch = false;
    WeaponType _cullentWeaponType;

    public WeaponType CullentWeaponType => _cullentWeaponType;

    void Awake()
    {
        _weaponDatas.Initialize(_weaponScriptableObjects);
        for (int i = 0; i < _weaponEquipment.Length; i++)
        {
            int index = i;
            _equipment.Add(_weaponEquipment[index]);
            WeaponEquipmentState(index);
        }
        WeaponProcessingState();
        _isSwitch = false;
        _weaponProcessing.SetWeapon(_weaponDatas.GetWeapon(WeaponType.SWORD), CommandType.MAIN);
        _weaponProcessing.SetWeapon(_weaponDatas.GetWeapon(WeaponType.LANCE), CommandType.SUB);
        _weaponProcessing.SetEquipment(_isSwitch);
        _weaponInput.TargetWeapon = _weaponProcessing.SwichWeapon(_isSwitch);
    }
    void WeaponEquipmentState(int index)
    {
        //ïêäÌÇÃëïîıèÓïÒ
        _weaponEquipment[index].MainWeapon.Skip(1)
            .Subscribe(mainWeapon =>
            {
                _weaponProcessing.SetWeapon(_weaponDatas.GetWeapon(mainWeapon), CommandType.MAIN);
                //_cullentWeaponType = mainWeapon;
                Debug.Log(mainWeapon);
            }).AddTo(this);
        _weaponEquipment[index].SubWeapon.Skip(1)
           .Subscribe(subWeapon =>
           {
               _weaponProcessing.SetWeapon(_weaponDatas.GetWeapon(subWeapon), CommandType.SUB);
               //_cullentWeaponType = subWeapon;
               Debug.Log(subWeapon);
           }).AddTo(this);
        _weaponEquipment[index].Element.Skip(1)
            .Subscribe(element =>
            {
                _weaponProcessing.SetElement(element);
                if (element != ElementType.RIGIT)
                {
                    //_weaponElementColor.IsActiveElement(_cullentWeaponType,element);
                }
            }).AddTo(this);
        _weaponEquipment[index].Equiped.Skip(1)
            .Subscribe(equiped => 
            {
                _weaponInput.TargetWeapon = _weaponProcessing.SetEquipment(_isSwitch);
                _weaponInput.TargetWeapon = _weaponProcessing.SwichWeapon(_isSwitch);
            });

    }
    void WeaponProcessingState()
    {
        _weaponInput.IsSwichWeapon.Skip(1)
           .Subscribe(isSwich =>
           {
               _weaponInput.TargetWeapon = _weaponProcessing.SwichWeapon(isSwich);
               _isSwitch = isSwich;
           }).AddTo(this);
    }
}
